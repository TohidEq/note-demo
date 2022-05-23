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
        //static 
        //private DBController dBController = new DBController();
        public string sortNotesBy = "Title";
        static List<string> showTypes = new List<string>() { "personal"};

        public Form1()
        {
            InitializeComponent();
        }

        

        
        


        //just for test
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            DBController dBController = new DBController();
            //DBController dBController = new DBController();
            showText.Text = "";

            //Console.WriteLine("The list of databases on this server is: ");
            //dBController = new DBController();
            foreach (var doc in dBController.GetNoteIdByNotThisTypes("work","other"))
            {
                //doc[0]<=>doc["_id"]
                //showText.Text += doc["note_id"] + Environment.NewLine;
                foreach (var docs in doc)
                {
                    showText.Text += docs + Environment.NewLine;
                    //showText.Text += docs.Value + Environment.NewLine;
                    //Console.WriteLine(db);
                }
                //dBController.db.
                showText.Text += Environment.NewLine;
                //Console.WriteLine(db);
            }
            //showText.Text = insertText.Text;
            showText.Text = showText.Text.Replace("\n", Environment.NewLine);

            //showText.Text = dBController.InsertToNote("s", "s", "s").ToString();



            //test for refresh:

            showText.Text = "xxxx";

           var x = dBController.GetTypesByNoteId(111400100.ToString());

            foreach(var s in x)
            {
                showText.Text += s+Environment.NewLine;
                foreach (var s2 in s)
                {
                    showText.Text += s2 + Environment.NewLine+"ss";
                }
            }
            



        }



        ////////////////
        // show notes //
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            DBController dBController = new DBController();
            //clear show text box
            showText.Text       = "";

            /*bool personalType   = showPersonal.     Checked,
                 workType       = showWork.         Checked,
                 otherType      = showOther.        Checked,
                 withoutType    = showWithoutType.  Checked;*/

            //dBController = new DBController();

            /*// sort by
            List<BsonDocument> allNotesId = dBController.GetNotesIdSortByTitle();*/

            List<string> allNotesId = new List<string>();
            

            switch (sortNotesBy)
            {
                case "Title":
                    foreach (var item in dBController.GetNotesSortByTitle())
                        allNotesId.Add(item["_id"].ToString());
                    break;

                case "Id":
                    foreach (var item in dBController.GetNotesSortById())
                        allNotesId.Add(item["_id"].ToString());
                    break;

                case "Type":
                    allNotesId = ListNotesIdByType();
                    break;
            }




            //test:
            foreach (var item in allNotesId)
            {   

                var noteTypes = dBController.GetTypesByNoteId(item);

                if (CheckTypes(item))
                { 
                    foreach (var note in
                                dBController.
                                GetNoteById(item)
                            )
                    {
                        foreach (var noteItem in note)
                        {
                            showText.Text += ">>"+noteItem.Name+": "+noteItem.Value + 
                                            Environment.NewLine;
                        }

                        showText.Text += ">>types:";
                        foreach (var types in noteTypes)
                        {
                            showText.Text += types["type"]
                            +", ";
                        }
                    }

                    showText.Text += Environment.NewLine + "-  -  -  -  -" + Environment.NewLine;
                }


            }

        }




        ////////////////////////////////////////////////
        // Click On Types (CheckBoxes (Show Section)) //
        private void ClickOnShowTypes(object sender, EventArgs e)
        {
            var btn = (sender as CheckBox);
            if (btn.Checked)
            {
                if (!showTypes.Contains(
                    (sender as CheckBox).Text)
                    )
                {
                    showTypes.Add((sender as CheckBox).Text);
                }
            }
            else
            {
                if (showTypes.Contains(
                    (sender as CheckBox).Text)
                    )
                {
                    showTypes.Remove((sender as CheckBox).Text);
                }
            }

        }










        //////////////////////
        // custom functions //

        static List<string> ListNotesIdByType()
        {
            DBController dBController = new DBController();
            List<string> result = new List<string>();

            foreach (var item in dBController.GetNoteIdByType("other"))
                result.Add(item["note_id"].ToString());

            foreach (var item in dBController.GetNoteIdByType("personal"))
                result.Add(item["note_id"].ToString());

            foreach (var item in dBController.GetNoteIdByType("work"))
                result.Add(item["note_id"].ToString());

            foreach (var item in dBController.GetNoteIdByType("without type"))
                result.Add(item["note_id"].ToString());

            //remove duplicate items
            result = result.Distinct().ToList();

            return result;
        }

        

        static bool CheckTypes(string noteId)
        {
            DBController dBController = new DBController();

            var noteTypes = dBController.GetTypesByNoteId(noteId);

            foreach(var item in showTypes)//get all (show type)check box checked...
            {
                foreach (var type in noteTypes)
                {
                    if (type["type"] == item)
                    {
                        return true;//founded
                    }
                        
                }
            }

            //not found
            return false;
        }




        ////////////////////
        // custom methods //


        //set sort method //
        private void SortClick(object sender, EventArgs e)
        {
            string x = (sender as RadioButton).Text;
            sortNotesBy = x;
        }



    }
}
