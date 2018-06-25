Imports System.IO
Imports System.Text

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedItem = ComboBox1.Items(0)
        LoadData()

    End Sub
    Private Sub LoadData()
        dirLabel.Text = My.Settings.ProjectsFolder
        For Each Project As String In Directory.GetDirectories(My.Settings.ProjectsFolder)
            ListBox1.Items.Add(Project)
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            My.Settings.ProjectsFolder = FolderBrowserDialog1.SelectedPath
            LoadData()
        End If
    End Sub
    Private Sub StartProject()
        Dim SelectedProject As String = ListBox1.SelectedItem.ToString
        Dim strarr() As String = SelectedProject.Split("\"c)
        Dim ProjectName As String = strarr(strarr.Length - 1)
        Dim executablePath As New StringBuilder
        executablePath.Append(SelectedProject)
        If ComboBox1.SelectedItem = "JetBrains CLion" Then
            executablePath.Append("\cmake-build-debug\")
            executablePath.Append(ProjectName)
            executablePath.Append(".exe")
            StartProgram(executablePath.ToString, ProjectName)
        End If
    End Sub
    Private Sub StartProgram(path As String, title As String)
        Process.Start("cmd", "/c title " + title + " && " + path + " && echo. && pause")
    End Sub
    Private Sub startBtn_Click(sender As Object, e As EventArgs) Handles startBtn.Click
        StartProject()
    End Sub
End Class
