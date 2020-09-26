using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam_Client
{
    public partial class StudentForm : Form
    {
        private NetworkStream output; // stream for receiving data           
        public BinaryWriter writer; // facilitates writing to the stream    
        private BinaryReader reader; // facilitates reading from the stream  
        private Thread readThread; // Thread for processing incoming messages
        private string message = "";
        private int ClientName;
        int QuestionCount=1;
        Wait w = new Wait();
        public StudentForm()
        {
            InitializeComponent();
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            readThread = new Thread(new ThreadStart(RunClient));
            readThread.Start();
        }
        private delegate void DisplayDelegate(string message);
        private void DisplayMessage(string message)
        {
            // if modifying displayTextBox is not thread safe
            if (displayTextBox.InvokeRequired)
            {
                // use inherited method Invoke to execute DisplayMessage
                // via a delegate                                       
                Invoke(new DisplayDelegate(DisplayMessage),
                   new object[] { message });
            } // end if
            else // OK to modify displayTextBox in current thread
                displayTextBox.Text += message;
        }
        public void RunClient()
        {
            TcpClient client;
            
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                string Ip="127.0.0.1";
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Ip = ip.ToString();
                    }
                }

                DisplayMessage("Attempting connection\r\n");
                
                client = new TcpClient();
                client.Connect(IPAddress.Parse(Ip), 50000);
                
                output = client.GetStream();
                
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);

                DisplayMessage("\r\nGot I/O streams\r\n");

                int count = 0;
                ClientName = reader.ReadInt32();
                DisplayMessage("\r\n I am Student : " + ClientName);
                this.Invoke((MethodInvoker)(() => this.Text = "StudentId <<" + ClientName + ">>"));

                do
                {
                    try
                    {
                        message = reader.ReadString();
                        DisplayMessage("\r\n" + message);

                        if(message == "Your Exam Started ...")
                        {
                            StartExam();
                        }
                        else if (message == "Exam Result")
                        {
                            this.Show();
                            GetExamResult();
                        }
                        else if (message == "Exam Not Completed")
                        {
                            groupBox1.Invoke((MethodInvoker)(() => groupBox1.Visible = false));
                            this.Show();
                            MessageBox.Show("Exam Not Completed !!");
                            QuestionCount = 1;
                        }
                    } 
                    catch (Exception)
                    {
                        DisplayMessage("\r\n" + "You are disconnected !!");
                        btn_startEx.Enabled = false;
                        message = "SERVER>>> TERMINATE";
                    } 

                    count++;
                } while (message != "SERVER>>> TERMINATE" && client.Connected);
                
                writer.Close();
                reader.Close();
                output.Close();
                client.Close();

               // Application.Exit();
            } 
            catch 
            {
                //MessageBox.Show(error.ToString(), "Connection Error",
                //   MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Environment.Exit(System.Environment.ExitCode);
                DisplayMessage("\r\n" + "The Server not Available !!");
                btn_startEx.Enabled = false;
            } // end catch
        }

        private void btn_startEx_Click(object sender, EventArgs e)
        {
            if(QuestionCount == 5) { QuestionCount = 1; }
            try
            {
                this.Hide();
                w.Show();
                writer.Write("Start Exam");
            }
            catch { }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                writer.Write("End Exam");
            }
            catch
            {

            }
            Thread.Sleep(1000);
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void StudentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            writer.Write("End Exam");
            Thread.Sleep(1000);
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void StartExam()
        {
            try
            {
                QUESTION Question = new QUESTION() { Id = reader.ReadInt32(), Question1 = reader.ReadString() };
                ANSWER Ans1 = new ANSWER() { Id = reader.ReadInt32(), Answer1 = reader.ReadString() };
                ANSWER Ans2 = new ANSWER() { Id = reader.ReadInt32(), Answer1 = reader.ReadString() };
                ANSWER Ans3 = new ANSWER() { Id = reader.ReadInt32(), Answer1 = reader.ReadString() };
                ANSWER Ans4 = new ANSWER() { Id = reader.ReadInt32(), Answer1 = reader.ReadString() };

                QuestionForm dd = new QuestionForm(Question, Ans1, Ans2, Ans3, Ans4, this, QuestionCount,w);
                dd.ShowDialog();
                QuestionCount++;
            }
            catch(Exception ex)
            {
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Visible = false));
                this.Show();
                MessageBox.Show("Error: >> "+ex.Message);
            }

        }
        private void GetExamResult()
        {
            try
            {
                bool Q1R, Q2R, Q3R, Q4R, Q5R;

                Q1R = reader.ReadBoolean();
                lbl_Q1.Invoke((MethodInvoker)(() => lbl_Q1.Text = Q1R ? "Correct" : "Wrong"));

                Q2R = reader.ReadBoolean();
                lbl_Q2.Invoke((MethodInvoker)(() => lbl_Q2.Text = Q2R ? "Correct" : "Wrong"));

                Q3R = reader.ReadBoolean();
                lbl_Q3.Invoke((MethodInvoker)(() => lbl_Q3.Text = Q3R ? "Correct" : "Wrong"));

                Q4R = reader.ReadBoolean();
                lbl_Q4.Invoke((MethodInvoker)(() => lbl_Q4.Text = Q4R ? "Correct" : "Wrong"));

                Q5R = reader.ReadBoolean();
                lbl_Q5.Invoke((MethodInvoker)(() => lbl_Q5.Text = Q5R ? "Correct" : "Wrong"));

                int r = reader.ReadInt32();

                lbl_Result.Invoke((MethodInvoker)(() => lbl_Result.Text = (r * 20) + " / 100"));
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Visible = true));

                QuestionCount = 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: >> "+ex.Message);
            }

        }
    }
}
