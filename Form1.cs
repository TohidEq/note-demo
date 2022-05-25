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
using System.Text.RegularExpressions;

namespace NoteDemo
{
    public partial class Form1 : Form
    {
        //static 
        //private DBController dBController = new DBController();
        private int q1 =0, q2 =0;
        public string sortNotesBy = "Title";
        static List<string> showTypes = new List<string>() { "personal"};
        public List<string> insertTypes = new List<string>();


        public Form1()
        {
            InitializeComponent();
        }


        //just for test
        private void buttonInsert_Clic()
        {
            DBController dBController = new DBController();
            //DBController dBController = new DBController();
            showText.Text = "";

            //Console.WriteLine("The list of databases on this server is: ");
            //dBController = new DBController();
            foreach (var doc in dBController.GetNoteIdByNotThisTypes("work", "other"))
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

            foreach (var s in x)
            {
                showText.Text += s + Environment.NewLine;
                foreach (var s2 in s)
                {
                    showText.Text += s2 + Environment.NewLine + "ss";
                }
            }




        }




        ///////////////////
        // insert method //
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            DBController dBController = new DBController();
            List<string> typesList = InsertTypes();  //get all selected types


            if(insertTitle.Text.Length>0 & insertText.Text.Length > 0)
            {
                string note_id = dBController.InsertToNote(
                                    insertTitle.Text,
                                    insertText.Text);
                foreach(var item in typesList)
                {
                    dBController.InsertToTypeNote(note_id, item);
                }


                // clear insert section
                ClearInsert();
                MessageBox.Show("successful. note id: "+ note_id);
            }
            else
            {
                MessageBox.Show("!!! write something pls !!!");
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

            showText.Text = showText.Text.Replace("\\n", "\r\n");

            MakeQuestion(); // for delete section

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



        /////////////////////////////////////
        // change answer text box value... //
        private void writeAnswer(object sender, EventArgs e)
        {
            if (Regex.IsMatch(checkA.Text, @"^[0-9]{1,2}$"))
                if (Convert.ToInt32(checkA.Text) == q1 + q2)
                {
                    if(buttonSendToUpdate.Enabled)
                        buttonDelete.Enabled = true;
                }
                else
                {

                    buttonDelete.Enabled = false;
                }
            else
            {

                buttonDelete.Enabled = false;
            }
        }
        
        /////////////////////////////////////
        // change note_id text box value... //
        private void writeNoteId(object sender, EventArgs e)
        {
            var dBController = new DBController();

            if (Regex.IsMatch(idText1.Text, @"^[0-9]{9}$") & Regex.IsMatch(idText2.Text, @"^[0-9]{9}$"))
                if (idText1.Text == idText2.Text )
                {
                    var validate = dBController.GetNoteById(idText1.Text);
                    
                    if (validate.Count() == 1)
                        buttonSendToUpdate.Enabled = true;
                    else
                        buttonSendToUpdate.Enabled = false;
                }
                else
                {

                    buttonSendToUpdate.Enabled = false;
                }
            else
            {

                buttonSendToUpdate.Enabled = false;
            }
        }


        ///////////////////////////////
        // delete note and note type //
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var dBController =new DBController();
            //idText1.Text;
            try
            {
                if (dBController.DeleteByNoteId(idText1.Text))
                {
                    MessageBox.Show("deleted. note id: " + idText1.Text);
                }
            }catch (Exception)
            {
                MessageBox.Show("error!!!");
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

        
        // for show section //
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


        // for insert section //
        public List<string> InsertTypes()
        {
            List<string> result = new List<string>();

            if (insertPersonal.Checked)
            {
                result.Add("personal");
            }
            if (insertWork.Checked)
            {
                result.Add("work");
            }
            if (insertOther.Checked)
            {
                result.Add("other");
            }
            if(!insertOther.Checked & !insertWork.Checked & !insertPersonal.Checked)
            {
                result.Add("without type");
            }
            return result;
        }



        ////////////////////
        // custom methods //


        // set sort method //
        private void SortClick(object sender, EventArgs e)
        {
            string x = (sender as RadioButton).Text;
            sortNotesBy = x;
        }

        // clear insert section method //
        private void ClearInsert()
        {
            insertTitle.Text = "";
            insertText.Text = "";
            insertPersonal.Checked = false;
            insertWork.Checked = false;
            insertOther.Checked = false;
        }

        

        private void MakeQuestion()
        {
            Random random  = new Random();
            q1 = Convert.ToInt32(random.Next(0, 9));
            q2 = Convert.ToInt32(random.Next(0, 9));
            checkQ.Text = q1.ToString() + " + "+ q2.ToString() + " = ?";
            buttonDelete.Enabled = false;
            checkA.Text = "";
        }

        
    }
}
