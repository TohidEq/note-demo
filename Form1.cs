using MongoDB.Driver;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        
        //just for test
        private void SortClick(object sender, EventArgs e)
        {
            string x = (sender as RadioButton).Text;
            showText.Text = x;
        }


        //just for test
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            

            showText.Text = "";

            //Console.WriteLine("The list of databases on this server is: ");
            DBController dBController = new DBController();
            foreach (var doc in dBController.GetNoteById(202205111356423))
            {
                //doc[0]<=>doc["_id"]
                showText.Text += doc["title"] + Environment.NewLine;
                /*foreach (var docs in doc)
                {

                    showText.Text += docs.Value + Environment.NewLine;
                    //Console.WriteLine(db);
                }*/
                //dBController.db.
                showText.Text += Environment.NewLine;
                //Console.WriteLine(db);
            }
            //showText.Text = insertText.Text;
            showText.Text = showText.Text.Replace("\n", Environment.NewLine);
        }
    }
}
