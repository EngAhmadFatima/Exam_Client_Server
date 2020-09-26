using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam_Srever
{
    public partial class ExServerForm : Form
    {


        private List<Student> Students;                
        private List<Thread> StudentThreads;
        private TcpListener listener;            
        private Thread getStudent;
        internal bool disconnected = false; 
        public Label lbl;
        public Label lbl_Id;
        public int ClientsCount { get; set; }


        public ExServerForm()
        {
            InitializeComponent();
        }

        private void ExServerForm_Load(object sender, EventArgs e)
        {
            //string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //MessageBox.Show(path);
            ClientsCount = 0;
            lbl = lbl_StudConnected;
            lbl_Id = lbl_ClientId;

            getStudent = new Thread(new ThreadStart(SetupSrever));
            getStudent.Start();
        }
     
        private delegate void DisplayDelegate(string message);

        public void DisplayMessage(string message)
        {
            if (displayTextBox.InvokeRequired)
            {                                  
                Invoke(new DisplayDelegate(DisplayMessage),
                   new object[] { message });
            } 
            else 
                displayTextBox.Text += message;
        } 

        public void SetupSrever()
        {
            Students = new List<Student>();
            StudentThreads = new List<Thread>();

            DisplayMessage("Waiting for Students...\r\n");
            listener = new TcpListener(IPAddress.Any, 50000);
            listener.Start();
            int i = 1;
            while (true)
            {
                Students.Add(new Student(listener.AcceptSocket(), this, i));
                StudentThreads.Add(new Thread(new ThreadStart(Students[i-1].Run)));
                StudentThreads[i-1].Start();
                ClientsCount++;
                i++;
            }
        }

        private void ExServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }

    public class Student
    {
        internal Socket connection;    
        private NetworkStream socketStream;  
        private ExServerForm server;        
        private BinaryWriter writer; 
        private BinaryReader reader; 
        private int number;                                           
        internal bool threadSuspended = true;


        public Student(Socket socket, ExServerForm serverValue, int newNumber)
        {
            connection = socket;
            server = serverValue;
            number = newNumber;
     
            socketStream = new NetworkStream(connection);
            writer = new BinaryWriter(socketStream);
            reader = new BinaryReader(socketStream);
        }

        public void Run()
        {

            server.lbl.Invoke((MethodInvoker)(() => server.lbl.Text = server.ClientsCount.ToString()));
            server.lbl_Id.Invoke((MethodInvoker)(() => server.lbl_Id.Text = number.ToString()));
            writer.Write(number);
            writer.Write("SERVER>>> Connection successful, you are Client:" + number);
            server.DisplayMessage("\r\nConnection " + number + " received. \r\n");

          var d = ((IPEndPoint)connection.LocalEndPoint).Address.ToString();

            server.DisplayMessage("\r\nAddress: " + d + " received. \r\n");

            List<int> CorrectAnswers = new List<int>(); // Fill in the Correct AnswerIds
            List<bool> QuestionResult = new List<bool>(); // Fill in the the result for each question (true => correct)
            List<int> UsedQuestions = new List<int>(); // Fill in used QuestionsId
            bool done = false;

            do
            {
                while (connection.Available == 0)
                {
                    Thread.Sleep(1000);
                    if (server.disconnected) return;
                }

                string message = reader.ReadString();

                // Starting Exam -------------------
                if (message == "Start Exam")
                {
                    StartExam(CorrectAnswers, UsedQuestions);
                }

                // Exam Ebding -----------------
                else if (message == "End Exam")
                {
                    done = true;
                    writer.Write("You End the Exam ...");
                    server.DisplayMessage("\r\nStudent (" + number + ") End the Exam ...");
                    server.ClientsCount--;
                }

                // Get Answers -------------
                else if (message == "Answer")
                {
                    GetAnswer(CorrectAnswers, QuestionResult);
                }

                // Exam Finishing ------------
                else if (message == "Finish Exam")
                {
                    FinishExam(QuestionResult);
                    UsedQuestions.Clear();
                    CorrectAnswers.Clear();
                    QuestionResult.Clear();
                }
                else server.DisplayMessage("\r\n Student " + number + " >> " + message);

            }
            while (!done && connection.Connected);

            server.DisplayMessage("\r\nStudent (" + number + ") Exit ...");
            server.lbl.Invoke((MethodInvoker)(() => server.lbl.Text = server.ClientsCount.ToString()));

            writer.Close();
            reader.Close();
            socketStream.Close();
            connection.Close();
            
        }

        private void StartExam(List<int> CorrectAnswers, List<int> UsedQuestion)
        {
            Random rnd = new Random();
            int num = rnd.Next(1, 15);
            
            _if:
            if (!UsedQuestion.Contains(num))
            {
                try
                {
                    
                    string path = Environment.CurrentDirectory;
                    string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+path+"\\EXAM_DB.accdb";
                    OleDbConnection con = new OleDbConnection(connectionString);

                    QUESTION q = new QUESTION();
                    con.Open();
                    OleDbCommand command = new OleDbCommand("SELECT * FROM QUESTIONS WHERE Id =" + num, con);

                    OleDbDataReader Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        q.Id = int.Parse(Reader["Id"].ToString());
                        q.Question1 = Reader["Question"].ToString();
                    }
                    con.Close();

                    
                    List<ANSWER> ans = new List<ANSWER>();
                    con.Open();
                    OleDbCommand command1 = new OleDbCommand("SELECT * FROM ANSWERS WHERE QuestionId =" + q.Id, con);

                    OleDbDataReader Reader1 = command1.ExecuteReader();
                    
                    while (Reader1.Read())
                    {
                        ANSWER ansr = new ANSWER();
                        ansr.Id = int.Parse(Reader1["Id"].ToString());
                        ansr.Answer1 = Reader1["Answer"].ToString();
                        ansr.Result = (bool)Reader1["Result"];
                        ans.Add(ansr);
                    }
                    con.Close();
                    
                    server.DisplayMessage("\r\nStudent (" + number + ") Get Question Id: " + q.Id);
                    writer.Write("Your Exam Started ...");
                    writer.Write(q.Id);
                    writer.Write(q.Question1);

                    writer.Write(ans[0].Id);
                    writer.Write(ans[0].Answer1);

                    writer.Write(ans[1].Id);
                    writer.Write(ans[1].Answer1);

                    writer.Write(ans[2].Id);
                    writer.Write(ans[2].Answer1);

                    writer.Write(ans[3].Id);
                    writer.Write(ans[3].Answer1);

                    foreach (var item in ans)
                    {
                        if ((bool)item.Result)
                        {
                            CorrectAnswers.Add(item.Id);
                        }
                    }

                    UsedQuestion.Add(q.Id);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                if (num == 16)
                {
                    num = 2;
                    goto _if;
                }
                else
                {
                    num = num + 1;
                    goto _if;
                }
            }
        }
        private void GetAnswer(List<int> CorrectAnswers, List<bool> QuestionResult)
        {
            int QusId = reader.ReadInt32();
            int AnsId = reader.ReadInt32();
            server.DisplayMessage("\r\nStudent (" + number + ") Send Answer Id: " + AnsId+ "\r\n");
            if (CorrectAnswers.Contains(AnsId))
            {
                QuestionResult.Add(true);
            }
            else { QuestionResult.Add(false); }
        }
        private void FinishExam(List<bool> QuestionResult)
        {
            if (QuestionResult.Count != 5)
            {
                writer.Write("Exam Not Completed");
            }
            else
            {
                int counter = 0;
                foreach (var item in QuestionResult)
                {
                    if (item)
                    {
                        counter++;
                    }
                }
                server.DisplayMessage("\r\nStudent (" + number + ") Finish Exam ...");
                writer.Write("Exam Result");
                writer.Write(QuestionResult[0]);
                writer.Write(QuestionResult[1]);
                writer.Write(QuestionResult[2]);
                writer.Write(QuestionResult[3]);
                writer.Write(QuestionResult[4]);
                writer.Write(counter);
                writer.Write("Your Result is: " + counter + " / 5");
                server.DisplayMessage("\r\nStudent (" + number + ") Exam Result: " +(counter*20)+ " / 100 \r\n");
            }


        }

    }
}
