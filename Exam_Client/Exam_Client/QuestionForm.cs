using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam_Client
{
    public partial class QuestionForm : Form
    {
        QUESTION Qu;
        ANSWER An1, An2, An3, An4;
        StudentForm Stu;
        int QusCount;
        Wait w;
        private void QuestionForm_Load(object sender, EventArgs e)
        {
            lbl_Qus.Text = Qu.Question1.Trim();

            radioButton1.Text = An1.Answer1.Trim();
            radioButton1.Tag = An1.Id;

            radioButton2.Text = An2.Answer1.Trim();
            radioButton2.Tag = An2.Id;

            radioButton3.Text = An3.Answer1.Trim();
            radioButton3.Tag = An3.Id;

            radioButton4.Text = An4.Answer1.Trim();
            radioButton4.Tag = An4.Id;

            this.Text = "Question "+ QusCount +" / 5";
            w.Hide();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stu.writer.Write("Finish Exam");
            this.Close();
        }

        public QuestionForm()
        {
            InitializeComponent();
        }
        public QuestionForm(QUESTION Q, ANSWER A1, ANSWER A2, ANSWER A3, ANSWER A4, StudentForm stu, int qusCount, Wait W)
        {
            InitializeComponent();
            Qu = Q;
            An1 = A1;
            An2 = A2;
            An3 = A3;
            An4 = A4;
            Stu = stu;
            QusCount = qusCount;
            w = W;
        }
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            int id=0;
            if (QusCount < 6)
            {
                
                if (radioButton1.Checked)
                {
                    id = int.Parse(radioButton1.Tag.ToString());
                }
                else if (radioButton2.Checked)
                {
                    id = int.Parse(radioButton2.Tag.ToString());
                }
                else if (radioButton3.Checked)
                {
                    id = int.Parse(radioButton3.Tag.ToString());
                }
                else if (radioButton4.Checked)
                {
                    id = int.Parse(radioButton4.Tag.ToString());
                }
                else
                {
                    MessageBox.Show("Pleas Choos the Answer !!");
                }

                if(id != 0)
                {
                    Stu.writer.Write("Answer");
                    Stu.writer.Write(Qu.Id);
                    Stu.writer.Write(id);

                    if (QusCount == 5)
                    {
                        Stu.writer.Write("Finish Exam");
                        this.Close();
                    }
                    else
                    {
                        Stu.writer.Write("Start Exam");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            
        }
    }
}
