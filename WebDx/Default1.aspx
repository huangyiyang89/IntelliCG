<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default1.aspx.cs" Inherits="WebDx._Default" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="Id" Theme="Moderno">
        <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowFilterBar="Auto"></Settings>

        <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowDelete="False"></SettingsDataSecurity>

        <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
        <Columns>
            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="1">
                <EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Price" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Count" VisibleIndex="4"></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn FieldName="DengJi" VisibleIndex="5"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ZhongLei" VisibleIndex="6"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Category1" VisibleIndex="7"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Category2" VisibleIndex="8"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NaiJiu" VisibleIndex="9"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NaiJiuMax" VisibleIndex="10"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShengMing" VisibleIndex="11"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MoLi" VisibleIndex="12"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="GongJi" VisibleIndex="13"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FangYu" VisibleIndex="14"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MinJie" VisibleIndex="15"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="JingShen" VisibleIndex="16"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HuiFu" VisibleIndex="17"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MeiLi" VisibleIndex="18"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BiSha" VisibleIndex="19"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MingZhong" VisibleIndex="20"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FanJi" VisibleIndex="21"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShanDuo" VisibleIndex="22"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MoGong" VisibleIndex="23"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="KangMo" VisibleIndex="24"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShiHua" VisibleIndex="25"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Du" VisibleIndex="26"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Zui" VisibleIndex="27"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HunShui" VisibleIndex="28"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="YiWang" VisibleIndex="29"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HunLuan" VisibleIndex="30"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StallX" VisibleIndex="31"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StallY" VisibleIndex="32"></dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:CrossGateConnectionString %>' SelectCommand="SELECT * FROM [Items]"></asp:SqlDataSource>
</asp:Content>
