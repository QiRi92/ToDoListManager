// Import necessary namespaces (libraries)
using System;                           // Basic C# functions (Console, Events, etc.)
using System.Collections.Generic;       // Lets us use List<T> to store tasks
using System.Drawing;
using System.IO;                        // Allows reading/writing files (for save/load)
using System.Windows.Forms;             // WinForms UI controls

namespace ToDoListManager
{
    public partial class Form1 : Form
    {
        // Step 4: Create a list (container) to store all tasks
        private List<TaskItem> tasks = new List<TaskItem>();

        // Constructor: runs when Form1 is created
        public Form1()
        {
            InitializeComponent();   // Builds the UI from Form Designer
        }

        // 🔹 Step 4: Add Task Button Click Event
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if the textbox is not empty
            if (!string.IsNullOrWhiteSpace(txtTask.Text))
            {


                // Create a new task with description from textbox, not completed yet
                TaskItem newTask = new TaskItem { Description = txtTask.Text, IsCompleted = false };

                // Add this task to our task list
                tasks.Add(newTask);

                // Refresh the ListBox to show updated tasks
                UpdateTaskList();

                // Clear the textbox for the next input
                txtTask.Clear();
            }
            else
            {
                // Warn user if they clicked Add with no text
                MessageBox.Show("Please enter a task description.");
            }
        }

        // 🔹 Step 4: Method to refresh ListBox with tasks
        private void UpdateTaskList()
        {
            lstTasks.Items.Clear();

            string filter = cmbFilter.SelectedItem?.ToString() ?? "All";

            foreach (var task in tasks)
            {
                if (filter == "All" ||
                    (filter == "Active" && !task.IsCompleted) ||
                    (filter == "Completed" && task.IsCompleted))
                {
                    lstTasks.Items.Add(task);
                }
            }
        }


        // 🔹 Step 5: Delete Selected Task
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex >= 0)
            {
                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this task?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // Check if something is selected in ListBox
                if (lstTasks.SelectedIndex >= 0)
                {
                    TaskItem selectedTask = tasks[lstTasks.SelectedIndex];

                    // Remove the task at that position
                    tasks.RemoveAt(lstTasks.SelectedIndex);

                    // Refresh ListBox
                    UpdateTaskList();

                }
            }
            else
            {
                // Warn if no task is selected
                MessageBox.Show("Please select a task to delete.");
            }
        }

        // 🔹 Step 6: Mark Task as Completed
        private void btnComplete_Click(object sender, EventArgs e)
        {
            // Check if something is selected
            if (lstTasks.SelectedIndex >= 0)
            {


                // Mark that task as completed
                tasks[lstTasks.SelectedIndex].IsCompleted = true;

                // Refresh ListBox to show [✔]
                UpdateTaskList();

            }
            else
            {
                MessageBox.Show("Please select a task to mark as complete.");
            }
        }

        // 🔹 Step 7: Save Tasks to a file (tasks.txt)
        private void SaveTasks()
        {
            // Create or overwrite "tasks.txt"
            using (StreamWriter writer = new StreamWriter("tasks.txt"))
            {
                // Write each task as "Description|IsCompleted"
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Description}|{task.IsCompleted}");
                }
            }
        }

        // 🔹 Step 7: Load Tasks from file (if exists)
        private void LoadTasks()
        {
            // Only try loading if file exists
            if (File.Exists("tasks.txt"))
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines("tasks.txt");

                foreach (string line in lines)
                {
                    // Split line into description and completed status
                    var parts = line.Split('|');

                    // Only accept valid lines
                    if (parts.Length == 2)
                    {
                        // Create new TaskItem and add to list
                        tasks.Add(new TaskItem
                        {
                            Description = parts[0],
                            IsCompleted = bool.Parse(parts[1])
                        });
                    }
                }

                // Update ListBox with loaded tasks
                UpdateTaskList();
            }
        }

        // 🔹 Step 7: Load tasks automatically when form opens
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0; // this selects "All" (the first item)
            LoadTasks();  // Load saved tasks

        }

        // 🔹 Step 7: Save tasks automatically when form closes
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTasks();  // Save current tasks
        }

        private void lstTasks_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            TaskItem task = (TaskItem)lstTasks.Items[e.Index];

            // Background
            e.DrawBackground();

            // Set font & color
            Font font = e.Font;
            Brush brush = Brushes.Black;

            if (task.IsCompleted)
            {
                font = new Font(e.Font, FontStyle.Regular);
                brush = Brushes.Green;
            }

            e.Graphics.DrawString(task.ToString(), font, brush, e.Bounds);

            e.DrawFocusRectangle();
        }

        private void lstTasks_DoubleClick(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex >= 0)
            {
                TaskItem task = tasks[lstTasks.SelectedIndex];
                string newDescription = Microsoft.VisualBasic.Interaction.InputBox(
                    "Edit Task:", "Edit Task", task.Description);


                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    task.Description = newDescription;
                    UpdateTaskList();
                }
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTasks.Items.Clear();
            foreach (var task in tasks)
            {
                if (cmbFilter.SelectedItem.ToString() == "All" ||
                    (cmbFilter.SelectedItem.ToString() == "Active" && !task.IsCompleted) ||
                    (cmbFilter.SelectedItem.ToString() == "Completed" && task.IsCompleted))
                {
                    lstTasks.Items.Add(task);
                }
            }
        }
    }
}