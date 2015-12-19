using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PhoneBookDao.Model;
using PhoneBookServices.Services;
using PhoneBookServices.Services.Impl;

namespace PhoneBook
{
    public partial class PhoneBook : Form
    {
        private const int ContactsLevel = 0;
        private const int PhonesLevel = 1;

        public PhoneBook()
        {
            InitializeComponent();
        }

        private readonly IContactServices _contactServices = new ContactServicesImpl();
        private readonly IPhoneNumberServices _phoneNumberServices = new PhoneNumberServicesImpl();

        private void HideParentPanel(object sender, EventArgs e)
        {
            (sender as Button)?.Parent.Hide();
            var textBoxs = (sender as Button)?.Parent.Controls.OfType<TextBox>();
            if (textBoxs == null) return;
            foreach (var control in textBoxs)
            {
                (control).Clear();
            }
        }

        private void ShowPanel(object sender, EventArgs e)
        {
            ((sender as Button)?.Tag as Panel)?.Show();
            ((sender as Button)?.Tag as Panel)?.BringToFront();
        }

        private readonly SortedList<Contact, List<PhoneNumber>> _phoneBookTree =
            new SortedList<Contact, List<PhoneNumber>>();

        private void PhoneBook_Load(object sender, EventArgs e)
        {
            this.X.Click += new System.EventHandler(this.HideParentPanel);
            this.button1.Click += new System.EventHandler(this.HideParentPanel);
            this.button2.Click += new System.EventHandler(this.HideParentPanel);
            this.button3.Click += new System.EventHandler(this.HideParentPanel);
            this.buttonAddContact.Click += new System.EventHandler(this.HideParentPanel);
            this.buttonEditContact.Click += new System.EventHandler(this.HideParentPanel);
            this.buttonEditPhoneNumber.Click += new System.EventHandler(this.HideParentPanel);
            this.buttonAddPhone.Click += new System.EventHandler(this.HideParentPanel);

            this.buttonMainAddContact.Click += new System.EventHandler(this.ShowPanel);
            this.buttonMainAddPhoneNumber.Click += new System.EventHandler(this.ShowPanel);
            this.buttonMainEditContact.Click += new System.EventHandler(this.ShowPanel);
            this.buttonMainEditNumber.Click += new System.EventHandler(this.ShowPanel);

            LoadTreePhone();
        }
        private void LoadTreePhone()
        {
            List<Contact> allContacts = _contactServices.GetAllContacts();
            allContacts.Sort();

            foreach (var contact in allContacts)
            {
                var phonesOnContact = _phoneNumberServices.GetNumbersByContact(contact);
                _phoneBookTree.Add(contact, phonesOnContact);

                treeViewPhoneTree.Nodes.Insert(_phoneBookTree.IndexOfKey(contact), contact.Id.ToString(), contact.Name);
                foreach (var number in phonesOnContact)
                {
                    treeViewPhoneTree.Nodes[contact.Id.ToString()].Nodes.Add(number.ToString());
                }
            }
        }

        private void treeViewPhoneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = treeViewPhoneTree.SelectedNode;

            if (selectedNode.Level.Equals(ContactsLevel))
            {
                buttonMainAddPhoneNumber.Enabled = true;
                buttonMainEditContact.Enabled = true;
                buttonMainEditNumber.Enabled = false;
            }
            if (selectedNode.Level.Equals(PhonesLevel))
            {
                buttonMainEditNumber.Enabled = true;
                buttonMainAddPhoneNumber.Enabled = false;
                buttonMainEditContact.Enabled = false;
            }
        }

        private void textBoxPhoneTreeFind_TextChanged(object sender, EventArgs e)
        {
            treeViewPhoneTree.SelectedNode =
                (from TreeNode treeNode in treeViewPhoneTree.Nodes
                    where treeNode.Text.ToLower().Contains(textBoxPhoneTreeFind.Text.ToLower())
                    orderby
                        treeNode.Text.ToLower()
                            .IndexOf(textBoxPhoneTreeFind.Text.ToLower(), StringComparison.Ordinal),
                        treeNode.Text.Length ascending
                    select treeNode)
                    .FirstOrDefault();
        }

        private void buttonAddContact_Click(object sender, EventArgs e)
        {
            var contact = new Contact {Name = textBoxAddContact.Text};
            contact.Id = _contactServices.InsertOrUpdateContact(contact);

            _phoneBookTree.Add(contact, new List<PhoneNumber>());
            treeViewPhoneTree.Nodes.Insert(_phoneBookTree.IndexOfKey(contact), contact.Name);
        }

        private void buttonAddPhone_Click(object sender, EventArgs e)
        {
            var selectedNode = treeViewPhoneTree.SelectedNode;
            var contact = _phoneBookTree.ElementAt(selectedNode.Index).Key;

            var phoneNumber = new PhoneNumber
            {
                Number = textBoxAddPhoneNumber.Text,
                Note = textBoxAddPhoneNote.Text,
                ContactId = contact.Id
            };

            phoneNumber.Id = _phoneNumberServices.InsertOrUpdateNumber(phoneNumber);
            _phoneBookTree[contact].Add(phoneNumber);
            selectedNode.Nodes.Add(phoneNumber.ToString());
        }

        private void buttonEditContact_Click(object sender, EventArgs e)
        {
            var selectedNode = treeViewPhoneTree.SelectedNode;

            selectedNode.Text = textBoxEditContact.Text;
            var contact = _phoneBookTree.ElementAt(selectedNode.Index).Key;
            contact.Name = textBoxEditContact.Text;
            _contactServices.InsertOrUpdateContact(contact);
        }

        private void buttonEditPhoneNumber_Click(object sender, EventArgs e)
        {
            var selectedNode = treeViewPhoneTree.SelectedNode;

            var phoneNumber = _phoneBookTree.ElementAt(selectedNode.Parent.Index).Value[selectedNode.Index];

            phoneNumber.Number = textBoxEditPhoneNumber.Text;
            phoneNumber.Note = textBoxEditPhoneNote.Text;

            _phoneNumberServices.InsertOrUpdateNumber(phoneNumber);
            selectedNode.Text = phoneNumber.ToString();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var selectedNode = treeViewPhoneTree.SelectedNode;

            var contact = _phoneBookTree.ElementAt(selectedNode.Index).Key;

            if (selectedNode.Level.Equals(ContactsLevel))
            {
                _contactServices.DeleteContact(contact);
                _phoneBookTree.Remove(contact);
            }
            else
            {
                var phone = _phoneBookTree.ElementAt(selectedNode.Parent.Index).Value[selectedNode.Index];
                _phoneBookTree[contact].RemoveAt(selectedNode.Index);
                _phoneNumberServices.DeletePhoneNumber(phone);
            }
            selectedNode.Remove();
        }
    }
}
