<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebDx.Default" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Theme="Moderno" Width="100%">
                <SettingsPager PageSize="20"></SettingsPager>

                <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowFilterBar="Auto" ShowHeaderFilterButton="True"></Settings>

                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowDelete="False"></SettingsDataSecurity>
                <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
                <Columns><dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="0" Caption="物品"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Price" VisibleIndex="1" Caption="价格"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Count" VisibleIndex="2" Caption="数量"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DengJi" VisibleIndex="3" Caption="等级"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ZhongLei" VisibleIndex="4" Caption="种类"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NaiJiu" VisibleIndex="5" Caption="耐久"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NaiJiuMax" VisibleIndex="6" Caption="耐久上限"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FanJi" VisibleIndex="7" Caption="反击"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MingZhong" VisibleIndex="8" Caption="命中"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BiSha" VisibleIndex="9" Caption="必杀"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MeiLi" VisibleIndex="10" Caption="魅力"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HuiFu" VisibleIndex="11" Caption="回复"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="JingShen" VisibleIndex="12" Caption="精神"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MinJie" VisibleIndex="13" Caption="敏捷"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FangYu" VisibleIndex="14" Caption="防御"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="GongJi" VisibleIndex="15" Caption="攻击"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MoLi" VisibleIndex="16" Caption="魔力"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ShengMing" VisibleIndex="17" Caption="生命"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ShanDuo" VisibleIndex="18" Caption="闪躲"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MoGong" VisibleIndex="19" Caption="魔攻"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="KangMo" VisibleIndex="20" Caption="抗魔"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ShiHua" VisibleIndex="21" Caption="石化"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Du" VisibleIndex="22" Caption="毒"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Zui" VisibleIndex="23" Caption="醉"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HunShui" VisibleIndex="24" Caption="昏睡"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="YiWang" VisibleIndex="25" Caption="遗忘"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HunLuan" VisibleIndex="26" Caption="混乱"></dx:GridViewDataTextColumn>
                    
                </Columns>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:CrossGateConnectionString %>' SelectCommand="SELECT [Name], [Price], [Count], [DengJi], [ZhongLei], [NaiJiu], [NaiJiuMax], [FanJi], [MingZhong], [BiSha], [MeiLi], [HuiFu], [JingShen], [MinJie], [FangYu], [GongJi], [MoLi], [ShengMing], [ShanDuo], [MoGong], [KangMo], [ShiHua], [Du], [Zui], [HunShui], [YiWang], [HunLuan] FROM [Items]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
