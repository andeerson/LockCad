Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.VisualBasic


Public Class FormLogin

    Private OdataAdap As OleDbDataAdapter
    Private OdataSet As DataSet
    Dim Conec As New OleDbConnection




    Private Sub FormLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtChave.Focus()


       

    End Sub

   

    Private Sub btnConectar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConectar.Click
        If txtChave.Text = "sky" And txtSenha.Text = "159753" Then
            FormPrincipal.Show()


            Me.Close()

        Else
            MessageBox.Show("Sua chave ou senha estão incorretas", "LockCad SEGURANÇA P/ SEUS ARQUIVOS!", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub tbnCancela_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbnCancela.Click
        Me.Close()

    End Sub

    
 

  
End Class