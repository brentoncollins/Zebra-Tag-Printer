﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagPrinter
{

    public partial class MainWindow : Form

    {
        // Public number for counting up the tags
        int totalnumber = 0;
        List<object[]> Result;
        object item = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        //Print button checks
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            totalnumber = 0;
            // Get list from grid list[0][0]
            var Result = myDataGridView.Rows.OfType<DataGridViewRow>().Select(
            r => r.Cells.OfType<DataGridViewCell>().Select(c => c.Value).ToArray()).ToList();

            // Set variable for the first item in the list
            var firstitem = Result[0][0];

            //If the first item is null throw message and return
            if (firstitem == null)
            {
                MessageBox.Show("No data entered into table to print.", "Important Message");
                return;
            }

            // Set the print options
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                // Set the print document soze for the print function, this may be different for the print preview for viewing reasons.
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 1000  , 6000);
                printDocument1.Print();
            }
        }

        // Paste the clipboard into the DataGridView *** Look into not pasting columns
        private void PasteClipboard() {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                string s = Clipboard.GetText();

                string[] lines = s.Replace("\n", "").Split('\r');

                try
                {
                    myDataGridView.Rows.Add(lines.Length - 1);
                    string[] fields;
                    int row = 0;
                    int col = 0;

                    foreach (string item in lines)
                    {
                        fields = item.Split('\t');
                        myDataGridView[col, row].Value = fields.First();
                        row++;
                    }

            } 
                catch (Exception e)
                {
                 Console.WriteLine("An error occurred: '{0}'", e);
                }
            }
        }

        // Reset the counter, clear the grid and paste
        private void buttonPaste_Click(object sender, EventArgs e) {
 
            myDataGridView.Rows.Clear();
            PasteClipboard();
        }

        // Reset the counter and clear the grid
        private void buttonClear_Click(object sender, EventArgs e) {

            myDataGridView.Rows.Clear();
        }

        // Print document function will loop for all pages until the 
        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
           
            // Get table results
            Result = myDataGridView.Rows.OfType<DataGridViewRow>().Select(
            r => r.Cells.OfType<DataGridViewCell>().Select(c => c.Value).ToArray()).ToList();
            // Set vairable for the isolation item in the list

            item = Result[totalnumber][0];
            // Set variable for the next item this is to check later on if the next page needs to be setup or it is null

            // Print all text to the page ** Look into try block
            try {
                // Add one to the counter for tag numbers
                totalnumber += 1;
                e.Graphics.DrawString(totalnumber.ToString(), new Font("Areal", 22, FontStyle.Bold), Brushes.Black, 80, 50);
                e.Graphics.DrawString(textPermitNumber.Text, new Font("Areal", 16, FontStyle.Bold), Brushes.Black,160, 380);
                e.Graphics.DrawString(textPermitBox.Text, new Font("Areal", 16, FontStyle.Bold), Brushes.Black, 240, 405);
                e.Graphics.DrawString(item.ToString(), new Font("Areal", 16, FontStyle.Bold), Brushes.Black, 50, 480); //Across,Down
                e.Graphics.DrawString(textPermitOfficer.Text, new Font("Areal", 16, FontStyle.Bold), Brushes.Black, 190, 535);
                e.Graphics.DrawString(textPermitIsoOfficer.Text, new Font("Areal", 16, FontStyle.Bold), Brushes.Black, 90, 580);
                e.Graphics.DrawString(DateTime.Now.ToString("d/M/yyyy"), new Font("Areal", 16, FontStyle.Bold), Brushes.Black, 90, 610);
                
                // Check if the next item is an empty string if it is the next tag is not needed.
                if (Result[totalnumber][0].Equals(null) || Result[totalnumber][0].Equals(""))
                {
                    e.HasMorePages = false;
                    totalnumber = 0;
                }
                //Else set up the next page
                else
                    e.HasMorePages = true;
            }
            // May not need this with the new check
            catch (Exception err)
            {
                Console.WriteLine("An error occurred: '{0}'", err);
            }
        }
        
        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        // Print Preview
        private void button1_Click(object sender, EventArgs e)
        {
  
            //Get table data to check if empty
            var Result = myDataGridView.Rows.OfType<DataGridViewRow>().Select(
            r => r.Cells.OfType<DataGridViewCell>().Select(c => c.Value).ToArray()).ToList();
        
            var firstitem = Result[0][0];

            if (firstitem == null)
            {
                MessageBox.Show("No data entered into table to preview.", "Important Message");
                return;
            }

            // Setup print preview
            //printPreviewDialog1.Size = new System.Drawing.Size(200, 300);
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 450, 700);
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
         
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();

           
        }

        private void textPermitBox_TextChanged(object sender, EventArgs e)
        {

        }

    }
}


