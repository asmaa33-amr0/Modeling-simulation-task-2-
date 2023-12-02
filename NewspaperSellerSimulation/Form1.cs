using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    public partial class Form1 : Form
    {
        task2 task2 = new task2();

        
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
         


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

         

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void display_table(List<SimulationCase> row)
        {

             
            dataGridView1.Rows.Clear();
            foreach (var item in row)
                dataGridView1.Rows.Add(item.DayNo, item.RandomNewsDayType, item.NewsDayType, item.RandomDemand,
                    item.Demand, item.SalesProfit, item.LostProfit, item.ScrapProfit, item.DailyNetProfit);
        }
        //run button------
        private void button2_Click_1(object sender, EventArgs e)
        {
            task2.Simulation_table();
           
            display_table(task2.get_data1().SimulationTable );
            //fill text boxes (performance measures)
            textBox8.Text = task2.get_data1().PerformanceMeasures.TotalSalesProfit.ToString();
            textBox8.ReadOnly = true;
            textBox2.Text = task2.get_data1().PerformanceMeasures.TotalNetProfit.ToString();
            textBox2.ReadOnly = true;
            textBox3.Text = task2.get_data1().PerformanceMeasures.DaysWithUnsoldPapers.ToString();
            textBox3.ReadOnly = true;
            textBox4.Text = task2.get_data1().PerformanceMeasures.TotalScrapProfit.ToString();
            textBox4.ReadOnly = true;
            textBox5.Text = task2.get_data1().PerformanceMeasures.DaysWithMoreDemand.ToString();
            textBox5.ReadOnly = true;
            textBox6.Text = task2.get_data1().PerformanceMeasures.TotalCost.ToString();
            textBox6.ReadOnly = true;
            textBox7.Text = task2.get_data1().PerformanceMeasures.TotalLostProfit.ToString();
            textBox7.ReadOnly = true;


        }
        //Browsing button-------
        private void button1_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
              
                string temp = selectedFilePath;
                string temp2="";
                

                //get the test file name only 
                for (int i = 130; i < temp.Length; i++)
                    temp2 += temp[i];
                textBox1.Text =temp2;
                textBox1.ReadOnly = true;
                task2.read_test_file(temp2);
            } 
            string file = textBox1.Text;              
             

        }
        public string test_case_name()
        {
            return textBox1.Text;
        }
       
        //test button--------
        private void Testing_Click(object sender, EventArgs e)
        {//Don't forget to change the test case number
            string t = TestingManager.Test(task2.get_data1(), test_case_name());
            MessageBox.Show(t);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}