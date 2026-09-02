<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompletedViewDoc.aspx.cs" Inherits="LogIn.Pages.CompletedViewDoc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet" />
    <link href="../CSS/Sales.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">



    <style>
        .container {
            display: block;
        }

        .full-width-flex {
            flex: 1; /* Makes the item take up the full available width */
            background-color: #001f3f;
            padding: 10px;
            width: 100%;
            color: white;
        }

        .wide-input {
            width: 100%;
            box-sizing: border-box;
            border-top: none;
            border-left: none;
            border-right: none;
        }


        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 8px;
            text-align: left;
            /* border: 1px solid #ddd;*/
        }

        .form-row {
            display: flex;
            width: 100%;
            box-sizing: border-box;
        }

            .form-row .label-container, .form-row .input-container {
                flex: 1;
                margin-right: 10px;
                box-sizing: border-box;
            }

                .form-row .label-container:last-child, .form-row .input-container:last-child {
                    margin-right: 0;
                }

        .label-container {
            width: 30%; /* Adjust width as needed */
            padding-right: 10px;
            text-align: right;
        }

        .input-container {
            width: 70%; /* Adjust width as needed */
            padding-left: 10px;
        }

        .full-width {
            width: 100%;
        }

        h5 {
            background-color: #001f3f;
            color: white;
            text-align: center;
            padding: 5px;
        }

        .submit-btn {
            background-color: #4CAF50;
            color: white;
            width: 100px !important;
        }

            .submit-btn:hover {
                background-color: #45a049;
            }

            .submit-btn:active {
                transform: scale(0.98);
            }

        .cancel-btn {
            background-color: #f44336;
            color: white;
            width: 100px !important;
        }

            .cancel-btn:hover {
                background-color: #e53935;
            }

            .cancel-btn:active {
                transform: scale(0.98);
            }




        .home-logo {
            width: 50px;
            height: 50px;
            border-radius: 50%;
            position: absolute;
            top: 10px;
            left: 10px;
            cursor: pointer;
        }

        .icon-button {
            background: none;
            border: none;
            padding: 0;
            font-size: 16px;
            cursor: pointer;
        }

            .icon-button i {
                color: #007bff; /* Change to your desired color */
            }

        .user-profile {
            position: absolute;
            top: 10px;
            right: 10px;
            display: flex;
            align-items: center;
            cursor: pointer;
        }

        .user-logo {
            width: 80px; /* Adjust the size */
            height: 45px;
            border-radius: 50%; /* Makes it circular */
        }

        .dropdown-arrow {
            font-size: 16px;
            margin-left: 5px;
        }

        .dropdown-content {
            display: none; /* Hidden by default */
            position: fixed;
            top: 50px; /* Adjust as needed */
            right: 0;
            background-color: white;
            border: 1px solid #ddd;
            padding: 10px;
            z-index: 1;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
        }

            .dropdown-content a {
                color: black;
                text-decoration: none;
                display: block;
            }

        .full-width {
            width: 100%;
        }

        .sal-grid .wide-input {
            width: 100% !important;
            max-width: none !important;
            box-sizing: border-box;
            border-top: none;
            border-left: none;
            border-right: none;
        }

        .full-width:focus {
            outline: none;
        }

        .full-width.invalid {
            border-color: red;
        }

        .user-profile:hover .dropdown-content {
            display: block; /* Show dropdown on hover */
        }

        .username-label {
            display: block;
            font-size: 14px;
            color: #333;
            margin-top: 5px;
            text-align: center;
        }

        .user-name {
            font-size: 1.5em;
            font-weight: bold;
            color: #333;
            top: 80px; /* Adjust this value as needed */
            right: 20px; /* Change right to left for better placement */
            position: fixed; /* Keeps it fixed in the viewport */
        }

        .nav-button {
            padding: 10px 20px;
            margin-left: 10px;
            font-size: 16px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            color: white;
        }

        .prev-button {
            background-color: #6c757d; /* Gray */
            width: 100px;
        }

        .next-button {
            background-color: #007bff; /* Blue */
            width: 100px;
        }

        .nav-button:hover {
            opacity: 0.9;
        }

        .left-image {
            width: 60%;
        }

        html, body {
            margin: 0;
            padding: 0;
            height: 100%;
            background: url('../Images/background.jpg') no-repeat center center fixed;
            background-size: cover;
            font-family: Arial, sans-serif;
            position: relative;
        }

            /* Watermark overlay */
            body::before {
                content: "";
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: url('../Images/Techative.png') no-repeat center center;
                background-size: 400px; /* adjust watermark size */
                opacity: 0.06; /* subtle watermark effect */
                z-index: 0;
                pointer-events: none; /* let clicks go through */
            }
    </style>
    <style>
        .custom-popup {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            display: none;
            justify-content: center;
            align-items: center;
            background-color: rgba(0, 0, 0, 0.5); /* dim background */
            z-index: 9999;
        }

        .custom-popup-content {
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            width: 400px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.3);
            position: relative;
        }

        .custom-popup-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 1px solid #ddd;
            margin-bottom: 15px;
        }

            .custom-popup-header h5 {
                margin: 0;
            }

        .close-popup {
            cursor: pointer;
            font-size: 20px;
            font-weight: bold;
        }

        .form-group {
            margin-top: 10px;
        }

        .custom-popup-body .form-group label {
            width: 80%; /* Ensure labels have a fixed width */
            text-align: right; /* Align the text to the right */
            margin-right: 180px; /* Add some space between the label and input */
        }

        .SaveBtn {
            padding: 6px 16px;
            background-color: #f44336;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .popup-textbox {
            width: 100%;
            height: 70px;
            border: 1px solid #ccc;
            border-radius: 6px;
            padding: 8px 10px;
            resize: none;
            font-size: 14px;
            margin-bottom: 20px;
        }

        .popup-label {
            font-weight: 500;
            color: #333;
            display: block;
            margin-bottom: 6px;
        }

        .popup-dropdown {
            width: 100%;
            height: 35px;
            border: 1px solid #ccc;
            border-radius: 6px;
            padding-left: 10px;
            font-size: 14px;
            margin-right: 70px;
        }

        /* Footer Button */
        .popup-actions {
            text-align: center;
            margin-top: 25px;
        }

        .popup-btn {
            background-color: #e63946;
            color: #fff;
            border: none;
            border-radius: 6px;
            padding: 8px 20px;
            font-size: 14px;
            cursor: pointer;
            transition: background 0.3s ease;
        }

            .popup-btn:hover {
                background-color: #c81e2f;
            }

        /* Animation */
        @keyframes popupFadeIn {
            from {
                opacity: 0;
                transform: scale(0.9);
            }

            to {
                opacity: 1;
                transform: scale(1);
            }
        }
        /* Prevent text wrapping */
        .nowrap-text {
            white-space: nowrap;
        }

        .left-align {
            text-align: left;
            display: block;
            width: 100%;
            margin-bottom: 6px;
            margin-right: 30px;
        }
    </style>


    <script type="text/javascript">
        document.addEventListener("keydown", function (e) {
            var activeElement = document.activeElement;

            // Ignore key presses if focus is in input, textarea, or select
            if (activeElement && (activeElement.tagName === "INPUT" ||
                activeElement.tagName === "TEXTAREA" ||
                activeElement.tagName === "SELECT")) {
                return;
            }

            // Left Arrow → trigger Previous button
            if (e.key === "ArrowLeft" || e.keyCode === 37) {
                var prevBtn = document.querySelector("input[type=submit][value='Previous'], button.prev-button");
                if (prevBtn) prevBtn.click();
            }

            // Right Arrow → trigger Next button
            if (e.key === "ArrowRight" || e.keyCode === 39) {
                var nextBtn = document.querySelector("input[type=submit][value='Next'], button.next-button");
                if (nextBtn) nextBtn.click();
            }
        });
        function BankDetailspopup() {
            const popup = document.getElementById('BankDetailspopup');
            if (popup) {
                popup.style.display = 'flex'; // Show the popup
            } else {
                console.error('Popup element not found.');
            }
        }

        function closePopup() {
            const popup = document.getElementById('BankDetailspopup');
            if (popup) {
                popup.style.display = 'none'; // Hide the popup
            } else {
                console.error('Popup element not found.');
            }
        }
        // Function to show the popup
        function showPopup() {
            document.getElementById("BankDetailspopup").style.display = "block";
        }

        // Function to close the popup
        function closePopup() {
            document.getElementById("BankDetailspopup").style.display = "none";
        }

    </script>

    <div class="powered-by" style="position: fixed; bottom: 10px; right: 10px; color: black; font-size: 13px; opacity: 0.7; display: flex; align-items: center; gap: 6px;">
        <p style="margin: 0;"><strong>Powered by</strong></p>
        <img src="../Images/Techative.png" alt="Techative Logo" style="height: 10px;" />
    </div>
    <asp:HiddenField ID="hfPageIndex" runat="server" Value="1" />
    <asp:Panel ID="pnlPage1" runat="server">
        <div class="full-width-flex1">
            <h3 style="margin-left: 340px;">Business Partner Registration Form</h3>
        </div>
        <asp:Label runat="server" Style="margin-top: 15px;" CssClass="label-container">Contact Person<span style="color:red">*</span></asp:Label>
        <asp:TextBox ID="ContactPerson" runat="server" placeholder="ContactPerson" ReadOnly="true"
            Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
        <h5>KYC Documents (Please attach Photocopy)</h5>

        <div class="sal-grid">
            <div style="overflow: auto; overflow-x: hidden;">
                <asp:GridView ID="gvKYCDocuments" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <%--  <%# Container.DataItemIndex + 1 %>--%>
                                <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Document" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <%# Eval("DocumentType") %><span style="color: red;">*</span>
                                <br />
                                <%# Eval("DocumentType").ToString() == "Bank Account" ? "(Cancelled Cheque)" : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Upload" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <%-- <asp:FileUpload ID="fileUpload1" runat="server"
                                    onchange='<%# string.Format("__doPostBack(this.name, \"{0}\"); return validateFileExtension(this);", Eval("DocumentType").ToString()) %>' />--%>


                                <asp:Label ID="lblFileName" runat="server" ForeColor="Green" Font-Italic="true" />

                                <asp:Label ID="DocumentName" runat="server">DocName</asp:Label>
                                <%-- <Span ID="DocumentName" runat="server" ReadOnly="true">Content</Span>--%>
                                <p class="note">Accepted documents are .jpg, .pdf</p>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnView" runat="server" CommandName="ViewFile" CommandArgument='<%# Eval("DocumentType") %>' OnClick="btnView_Click" CssClass="icon-button">
<i class="fas fa-eye"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnDownload" runat="server" CommandName="DownloadFile" CommandArgument='<%# Eval("DocumentType") %>' OnClick="btnDownload_Click" CssClass="icon-button">
<i class="fas fa-download"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


            </div>

        </div>
        <asp:Panel ID="Panel1" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button5" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button6" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 1</strong></p>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlPage2" runat="server">
        <div class="full-width-flex1" style="margin-top: 0px !important;">
            <div class="full-width-flex" style="text-align: center">
                <strong>Business Partner Information</strong>
            </div>

        </div>
        <div>

            <table>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="GSTNo">GST Number<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox
                            ID="GSTNumber"
                            runat="server"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;"
                            AutoPostBack="True"
                            oninput="this.value = this.value.toUpperCase()"
                            onblur="validateGSTNumber(this)"
                            placeholder="Enter 15-character GST Number">
                        </asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="GSTNo">PAN Number<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="PANNumber" runat="server" placeholder="PAN Number" ReadOnly="true"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="partnerType">Partner Type<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="ddpartnertype" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                           
                        </asp:TextBox>
                    </td>
                </tr>
                <tr class="form-row">
                    <%--  <td class="label-container">
                        <label for="partnerType">Partner Type<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:DropDownList ID="ddpartnertype" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                            <asp:ListItem Text="Select Type" Value="Select Type"></asp:ListItem>
                            <asp:ListItem Text="Vendor" Value="Vendor"></asp:ListItem>
                            <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                    <%-- <td class="label-container">
                        <label for="tradeName">Trade Name<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="tradeName" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>--%>
                </tr>


                <tr class="form-row">
                    <td class="label-container">
                        <label for="registeredOfficeAddress1">Registered Office Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="registeredOfficeAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="registeredOfficeAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="registeredOfficeAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Registered Office Country Dropdown -->
                        <asp:TextBox ID="registeredOfficeCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox><br>

                        <!-- Registered Office State Dropdown -->

                        <asp:TextBox ID="registeredOfficeState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox>
                        <asp:TextBox ID="registeredOfficeCity" runat="server" CssClass="full-width" Placeholder="City" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="registeredOfficeZipCode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <div style="margin-top: 15px;">
                            <asp:CheckBox ID="sameAsRegisteredOffice" runat="server" Text="Same as Registered Office Address" onclick="copyAddress()" />
                        </div>
                    </td>
                    <td class="label-container">
                        <label for="goodsReturnAddress1">Goods Return Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="goodsReturnAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Registered Office Country Dropdown -->
                        <asp:TextBox ID="goodsReturnCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox><br>

                        <!-- Registered Office State Dropdown -->

                        <asp:TextBox ID="goodsReturnState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox>
                        <asp:TextBox ID="goodsReturnCity" runat="server" CssClass="full-width" Placeholder="City" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="goodsReturnZipcode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>

                    </td>


                    <td class="label-container">
                        <label for="businessBillingAddress1">Business / Billing Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="businessBillingAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="businessBillingAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="businessBillingAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Business Billing Country Dropdown -->

                        <asp:TextBox ID="businessBillingCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox><br>
                        <!-- Business Billing State Dropdown -->


                        <asp:TextBox ID="businessBillingState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:TextBox>
                        <asp:TextBox ID="businessBillingCity" runat="server" Placeholder="City" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="businessBillingZipCode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="tradeName">Trade Name<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="tradeName" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="natureOfBusinessActivity">Nature of Business Activity<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="natureOfBusinessActivity" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="dateOfEstablishment">Date of Establishment<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="dateOfEstablishment" runat="server" TextMode="Date" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">

                    <td class="label-container">
                        <label for="contactPersonName">Contact Person Name<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="contactPersonName" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="designation">Designation<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="designation" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="emailId">E-Mail ID<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="emailId" runat="server" TextMode="Email" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">
                    <%-- <td class="label-container">
                        <label for="emailId">E-Mail ID<span style="color: red;">*</span></label>
                    </td>--%>
                    <%-- <td class="input-container" colspan="3">
                        <asp:TextBox ID="emailId" runat="server" TextMode="Email" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>--%>
                    <td class="label-container">
                        <label for="mobileNo">Mobile No<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">

                        <asp:TextBox
                            ID="mobileNo"
                            runat="server"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;"
                            onblur="validateMobileNo(this)"
                            placeholder="Enter 10-digit mobile number">
                        </asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="officeTelephoneNo">Office Telephone No<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="officeTelephoneNo" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="tanNo">TAN Number<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="tanNo" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                </tr>





            </table>
        </div>
        <asp:Panel ID="Panel2" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button7" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button8" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 2</strong></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPage3" runat="server">
        <div>
            <table>
                <tr class="form-row" style="margin-left: 20px; background-color: #001f3f; align-content: center">
                    <td style="background-color: #001f3f; color: white; margin-left: 380px;">
                        <strong>MSME Details</strong>
                    </td>
                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="msmeRegistrationStatus" style="">MSME Registration Status<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="msmeRegistrationStatus" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;">
                           
                        </asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="MSMENO">MSME Number</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="MSMENO" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="ddlEnterpriseType">Enterprise Type</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="ddlEnterpriseType" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;">
                           
                        </asp:TextBox>
                    </td>
                    <td class="label-container"></td>

                    <td class="input-container"></td>
                </tr>
                <tr class="form-row" style="margin-left: 20px; background-color: #001f3f; align-content: center">
                    <td style="background-color: #001f3f; color: white; margin-left: 380px;">
                        <strong>Commercial Details</strong>
                    </td>
                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="CreditDays">Credit Days</label>
                    </td>
                    <td>
                        <asp:TextBox ID="CreditDays" runat="server" TextMode="MultiLine"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none; margin-right: 95px !important;">
                        </asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="DisCount">Bill Level Discount</label>
                    </td>
                    <td>
                        <asp:TextBox ID="DisCount" runat="server"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none; margin-right: 105px !important;">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="Payment1" style="">
                            Mark Down % on MRP
            <br>
                            (with Tax @ 0%)<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="Payment1" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="Payment2">
                            Mark Down % on MRP
             <br>
                            (with out Tax @ 0%</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="Payment2" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="Payment3" style="">
                            Mark Down % on MRP<br>
                            (with Tax @ 3%)<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="Payment3" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="Payment4">
                            Mark Down % on MRP
             <br>
                            (with out Tax @ 3%</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="Payment4" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="Payment5" style="">
                            Mark Down % on MRP<br>
                            (with Tax @ 5%)<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="Payment5" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="Payment6">
                            Mark Down % on MRP
             <br>
                            (with out Tax @ 5%</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="Payment6" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>
                <%-- <tr class="form-row">
                    <td class="label-container">
                        <label for="Payment7" style="">
                            Mark Down % on MRP<br>
                            (with Tax @ 12%)<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="Payment7" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="Payment8">
                            Mark Down % on MRP
             <br>
                            (with out Tax @ 12%</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="Payment8" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>--%>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="Payment9" style="">
                            Mark Down % on MRP<br>
                            (with Tax @ 18%)<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="Payment9" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="Payment10">
                            Mark Down % on MRP
             <br>
                            (with out Tax @ 18%</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="Payment10" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="BusinessType" style="">Type Of Vendor<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="BusinessType" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="AgencyEmail">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="AgencyEmail" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="AgencyName">Agency Name</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="AgencyName" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="AgencyEmail" style="visibility: hidden">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none; visibility: hidden"></asp:TextBox>
                    </td>

                </tr>
            </table>
        </div>
        <div class="sal-grid">
            <div style="overflow: auto; overflow-x: hidden;">
                <asp:GridView ID="GridView1" runat="server"
                    AutoGenerateColumns="false" CssClass="table table-responsive" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server"
                                    Style="display: block; text-align: center;"
                                    Text='<%# Container.DataItemIndex + 1 %>' />
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Document" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <%# string.IsNullOrEmpty(Eval("DocumentType") as string) ? "Performa Invoice" : Eval("DocumentType") %>
                                <span style="color: red;">*</span>
                                <br />
                                <%# (Eval("DocumentType") != null && Eval("DocumentType").ToString() == "Bank Account") ? "(Cancelled Cheque)" : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Upload" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>

                                <asp:Label ID="lblFileName" runat="server" ForeColor="Green" Font-Italic="true" />
                                <asp:Label ID="DocumentName" runat="server">DocName</asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnView" runat="server" CommandName="ViewFile"
                                    CommandArgument='<%# Eval("DocumentType") %>'
                                    OnClick="btnView_Click1" CssClass="icon-button">
                    <i class="fas fa-eye"  style="margin-left:13px;"></i>
                        </asp:LinkButton>

                                <asp:LinkButton ID="btnDownload" runat="server" CommandName="DownloadFile"
                                    CommandArgument='<%# Eval("DocumentType") %>'
                                    OnClick="btnDownload_Click1" CssClass="icon-button">
                    <i class="fas fa-download" style="margin-left:13px;"></i>
                        </asp:LinkButton>


                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="HiddenField2" runat="server" />
            </div>
        </div>
        <asp:Panel ID="Panel7" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button11" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button12" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 3</strong></p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlPage4" runat="server">
        <div>
            <table>

                <tr class="form-row" style="margin-left: 20px; background-color: #001f3f; align-content: center">
                    <td style="background-color: #001f3f; color: white; margin-left: 380px;">
                        <strong>Bank Account Details (Mandatory for Business Partner)</strong>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="bankName">Name of Bank:<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">

                        <asp:TextBox ID="bankName" runat="server" CssClass="full-width"
                            Style="width: 255px; border-top: none; border-left: none; border-right: none; margin-right: 45px; float: right;">
                        </asp:TextBox>

                    </td>

                    <td class="label-container">
                        <label for="accountName">Account Name in Bank:<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="accountName" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="accountNumber">Account Number:<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="accountNumber" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="ifscCode">IFSC Code:<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="ifscCode" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="branchCode">Branch Code:</label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="branchCode" runat="server" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="bankAddress">Bank Address:</label>
                    </td>
                    <td class="input-container">
                        <asp:TextBox ID="bankAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="full-width-flex11">
            <%-- <h3 style="margin-left: 340px;">Business Partner Registration Form</h3>--%>
        </div>

        <div style="margin-left: 20px">
            <h5>Business Location</h5>

            <div class="sal-grid">
                <div style="overflow: auto; overflow-x: hidden;">
                    <asp:GridView ID="gvProjectDetails" runat="server" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" OnRowDataBound="gvProjectDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />

                                    <asp:HiddenField ID="HiddenField11" runat="server" />
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Business State" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <%-- <asp:TextBox ID="businessState" runat="server" Style="width: 240px; border-top: none; border-left: none; border-right: none;" Text='<%# Bind("businessState") %>'></asp:TextBox>--%>
                                    <asp:TextBox ID="businessState" runat="server" CssClass="full-width"
                                        Style="width: 205px; border-top: none; border-left: none; border-right: none;">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GST Number" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <%-- <asp:TextBox ID="gstNumber" runat="server" Style="width: 240px; border-top: none; border-left: none; border-right: none;" Text='<%# Bind("businessState") %>'></asp:TextBox>--%>
                                    <asp:TextBox
                                        ID="gstNumber"
                                        runat="server"
                                        Style="width: 200px; border-top: none; border-left: none; border-right: none;"
                                        Text='<%# Bind("businessState") %>'
                                        oninput="this.value = this.value.toUpperCase()"
                                        onblur="validateGstNumber(this)"
                                        placeholder="Enter 15-character GST Number">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Address of Place" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:TextBox ID="addressOfPlace" runat="server" Style="width: 220px; border-top: none; border-left: none; border-right: none;" Text='<%# Bind("businessState") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GST Classification" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:TextBox ID="gstVendorClassification" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    <%--  <asp:LinkButton ID="lnkaddRow_Click" runat="server" ForeColor="Green" OnClick="lnknewrowadd_Click" OnClientClick="saveScrollPosition();">
            <img src="../Images/PlusIcon.png" style="width:18px;" />
                </asp:LinkButton>--%>
                    <asp:HiddenField ID="HiddenScrollPosition" runat="server" />
                </div>
            </div>



            <h5>Partners/Proprietor/Director's / Business Head Detail (Provide at Least One Person Details)</h5>

            <div class="sal-grid">
                <div style="overflow: auto; overflow-x: hidden;">
                    <asp:GridView ID="gvPartners" runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-responsive"
                        ShowHeaderWhenEmpty="true"
                        OnRowDataBound="gvPartners_RowDataBound"
                        DataKeyNames="RowID">
                        <Columns>

                            <asp:TemplateField HeaderText="S.No" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />

                                    <asp:HiddenField ID="HiddenField11" runat="server" />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:TextBox ID="partnerName" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:TextBox ID="partnerDesignation" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact_No" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <%--                                <asp:TextBox ID="partnerContactNo" runat="server" Style="width: 240px; border-top: none; border-left: none; border-right: none"></asp:TextBox>--%>
                                    <%--<asp:CustomValidator ID="cvMobileNo1" runat="server"
                                    ErrorMessage="Mobile number must be exactly 10 digits."
                                    ClientValidationFunction="validateMobileNumber"
                                    Display="Dynamic" ForeColor="Red"></asp:CustomValidator>--%>
                                    <asp:TextBox
                                        ID="partnerContactNo"
                                        runat="server"
                                        Style="width: 200px; border-top: none; border-left: none; border-right: none;"
                                        onblur="validatePartnerContactNo(this)"
                                        placeholder="Enter 10-digit mobile number">
                                    </asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email_ID" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:TextBox ID="partnerEmail" runat="server" TextMode="Email" Style="width: 200px; border-top: none; border-left: none; border-right: none" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                <%--  <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server" OnClick="lnkDelete_Click" ForeColor="Red">
                            <i class="fa fa-close"></i>
                        </asp:LinkButton>--%>
                            <%--                    </ItemTemplate>--%>
                            <%--</asp:TemplateField>>--%>
                        </Columns>
                    </asp:GridView>

                    <asp:HiddenField ID="HiddenField4" runat="server" />
                    <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkaddrow_Click" ForeColor="Green">
            <img src="../Images/PlusIcon.png" style="width:18px;" />
        </asp:LinkButton>--%>
                </div>
            </div>
        </div>
        <asp:Panel ID="Panel3" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button3" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button4" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 4</strong></p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlPage5" runat="server" Visible="false">

        <h5>Primary Operational Contacts</h5>

        <div class="sal-grid">
            <div style="overflow: auto; overflow-x: hidden;">
                <%--<asp:GridView ID="gvOperationalContacts" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Department">
                            <ItemTemplate>
                                
                                <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department") %>'></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:TextBox ID="pocName" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:TextBox ID="pocDesignation" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact No.">
                            <ItemTemplate>
                                <asp:TextBox ID="pocContactNo" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email-ID">
                            <ItemTemplate>
                                <asp:TextBox ID="pocEmail" runat="server" TextMode="Email" Style="width: 240px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
                <asp:GridView ID="gvOperationalContacts" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvOperationalContacts_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Department" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="pocName" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="pocDesignation" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact No." HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <%--<asp:TextBox ID="pocContactNo" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                                <asp:CustomValidator ID="cvMobileNo2" runat="server"
                                    ErrorMessage="Mobile number must be exactly 10 digits."
                                    ClientValidationFunction="validateMobileNumber"
                                    Display="Dynamic" ForeColor="Red">
</asp:CustomValidator>--%>

                                <asp:TextBox
                                    ID="pocContactNo"
                                    runat="server"
                                    Style="width: 200px; border-top: none; border-left: none; border-right: none;"
                                    onblur="validatePocContactNo(this)"
                                    placeholder="Enter 10-digit mobile number">
                                </asp:TextBox>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email-ID" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="pocEmail" runat="server" TextMode="Email" Style="width: 240px; border-top: none; border-left: none; border-right: none" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


            </div>
        </div>

        <h5>Major goods & services Dealt With </h5>

        <div class="sal-grid">
            <div style="overflow: auto; overflow-x: hidden;">
                <asp:GridView ID="gvMajorGoods" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvMajorGoods_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />

                                <asp:HiddenField ID="HiddenField33" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProduct" runat="server" Style="width: 140px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Brand" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBrand" runat="server" Style="width: 100px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSize" runat="server" Style="width: 100px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Material Description" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMaterialDescription" runat="server" Style="width: 140px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HSN Code" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtHSNCode" runat="server" Style="width: 100px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax %" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTaxPercentage" runat="server" TextMode="Number" Style="width: 90px; border-top: none; border-left: none; border-right: none;" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnView" runat="server" OnClientClick="openUploadPopup(this); return false;" CssClass="icon-button">
                            <i class="fas fa-eye"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:HiddenField ID="HiddenField3" runat="server" />
                <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkAddRow_Click" ForeColor="Green">
            <img src="../Images/PlusIcon.png" style="width:18px;" /> Add Row
        </asp:LinkButton>--%>
            </div>
        </div>
        <asp:Panel ID="Panel4" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button9" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button10" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 5</strong></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPage6" runat="server" Visible="false">
        <h5>List of Major Customers</h5>
        <asp:GridView ID="gvMajorCustomers" runat="server" AutoGenerateColumns="false"
            CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvMajorCustomers_RowDataBound">
            <Columns>

                <asp:TemplateField HeaderText="Sl. No." HeaderStyle-CssClass="center-header">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />
                        <%-- <%# Container.DataItemIndex + 1 %>--%>
                        <asp:HiddenField ID="HiddenField22" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="center-header">
                    <ItemStyle Width="70%" />
                    <HeaderStyle Width="70%" />
                    <ItemTemplate>
                        <div style="width: 100%;">
                            <asp:TextBox ID="customerName" runat="server" CssClass="wide-input"
                                Style="width: 100% !important; max-width: none !important; box-sizing: border-box; border-top: none; border-left: none; border-right: none;" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>


        <h5>Other Information</h5>
        <div class="sal-grid">
            <%--<div style="overflow: auto; overflow-x: hidden;">
                <asp:GridView ID="gvOtherInformation" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValue" runat="server" Style="width: 540px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>--%>
            <asp:GridView ID="gvOtherInformation" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvOtherInformation_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sl. No." HeaderStyle-CssClass="center-header">
                        <ItemTemplate>
                            <%--  <%# Container.DataItemIndex + 1 %>--%>
                            <asp:Label ID="lblSerialNo" runat="server" Style="display: block; text-align: center;" Text='<%# Container.DataItemIndex + 1 %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="center-header">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Value" HeaderStyle-CssClass="center-header">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValue" runat="server"
                                Style="width: 100% !important; max-width: none !important; box-sizing: border-box; border-top: none; border-left: none; border-right: none;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Panel ID="Panel5" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button1" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button2" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 6</strong></p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlPage7" runat="server" Visible="false">
        <div class="full-width-flex1">
            <h3 style="margin-left: 340px;">Business Partner Registration Form</h3>
        </div>

        <div style="margin-left: 20px">
            <p>
                I declare that the information furnished above is correct to the best of my knowledge. I undertake to inform the company immediately of any changes in the details as mentioned above.
         
            </p>

            <p>
                Name:<span style="color: red;">*</span>

                <asp:TextBox ID="declarationName" runat="server" Style="margin-left: 80px"></asp:TextBox>
            </p>
            <p>
                Designation:<span style="color: red;">*</span>

                <asp:TextBox ID="declarationDesignation" runat="server" Style="margin-left: 38px"></asp:TextBox>
            </p>
        </div>
        <asp:Panel ID="pnlNavigation" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 7</strong></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlApprovalButtons" runat="server" Style="margin-left: 18px; margin-bottom: auto" Visible="false">
        <asp:Button ID="Approve" runat="server" Text="Approve"
            ToolTip="Approve" Width="100px" Height="36px"
            Style="padding: 5px; border-radius: 3px; background-color: green; color: white; border: none; cursor: pointer; visibility: hidden;" Visible="false"
            OnClientClick="openApproveModalPage(); return false;" />
        <%--OnClick="ApproveVendor"--%>
        <asp:Button ID="btnReject" runat="server" Text="Reject"
            ToolTip="Reject" Width="100px" Height="36px"
            OnClick="Reject"
            OnClientClick="BankDetailspopup(); return false;"
            Style="padding: 5px; border-radius: 3px; background-color: #f44336; color: white; border: none; cursor: pointer; visibility: hidden;" Visible="false" />
        <%--<asp:Button ID="btnReject" runat="server" Text="Reject"
            ToolTip="Reject" Width="100px" Height="36px"
            OnClientClick="showRejectModal(); return false;"
            Style="padding: 5px; border-radius: 3px; background-color: #f44336; color: white; border: none; cursor: pointer;" />--%>
        <asp:Button ID="btnDraftApproved" runat="server" Text="Approve"
            ToolTip="DraftApproved" Width="100px" Height="36px"
            OnClick="DraftApproved"
            Style="padding: 5px; border-radius: 3px; background-color: forestgreen; color: white; border: none; cursor: pointer;" Visible="false" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel"
            ToolTip="Cancel" Width="100px" Height="36px"
            OnClick="Cancel"
            Style="padding: 5px; border-radius: 3px; background-color: #f44336; color: white; border: none; cursor: pointer;" Visible="false" />
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 3</strong></p>
        </div>
    </asp:Panel>

    <div id="BankDetailspopup" class="custom-popup" style="display: none;">
        <div class="custom-popup-content modern-popup">
            <!-- Header -->
            <div class="custom-popup-header">
                <h4 class="popup-title">Reason</h4>
                <span class="close-popup" onclick="closePopup1()">&times;</span>
            </div>

            <!-- Body -->
            <div class="custom-popup-body">
                <!-- Reason TextBox -->
                <asp:TextBox
                    ID="popuptext"
                    runat="server"
                    CssClass="popup-textbox"
                    placeholder="Enter your reason here">
                </asp:TextBox>

                <!-- GST Recreate Dropdown -->
                <div class="form-group">
                    <%--<label for="ddlGstRecreate" class="popup-label">Is this GST allowed to be recreated?</label>--%>
                    <label for="ddlGstRecreate" class="popup-label nowrap-text left-align">
                        Is this GST allowed to be recreated?
                    </label>
                    <asp:DropDownList
                        ID="ddlGstRecreate"
                        runat="server"
                        CssClass="popup-dropdown">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Yes" Value="Yes" />
                        <asp:ListItem Text="No" Value="No" />
                    </asp:DropDownList>
                </div>

                <!-- Action Button -->
                <div class="popup-actions">
                    <asp:Button
                        ID="SubmitButton1"
                        runat="server"
                        CssClass="popup-btn"
                        OnClick="SubmitButton_Click"
                        Text="OK" />
                </div>
            </div>
        </div>
    </div>

    <!-- Image Upload Modal: View Only -->
    <div id="imageUploadModal" class="custom-modal">
        <div class="custom-modal-content">
            <span class="close-btn" onclick="closeUploadPopup()">&times;</span>
            <h3 style="margin-bottom: 15px;">View Images</h3>

            <input type="hidden" id="popupSerialNo" />
            <input type="hidden" id="popupProduct" />

            <div id="previewContainer" class="thumb-container"></div>
        </div>
    </div>

    <!-- Full Image Preview Modal -->
    <div id="fullImageModal" class="custom-modal">
        <div class="custom-modal-content full-view">
            <span class="close-btn" onclick="closeFullImage()">&times;</span>
            <img id="fullImageView" src="" />
        </div>
    </div>

    <!-- Approval Remarks Modal -->
    <div class="modal fade" id="approveModalPage" tabindex="-1" aria-labelledby="approveModalLabelPage" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="approveModalLabelPage">Approval Remarks</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtRemarksPage" class="form-label">Enter Remarks:</label>
                        <asp:TextBox ID="txtRemarksPage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSubmitApprovalPage" runat="server" Text="Submit"
                        CssClass="btn btn-success" OnClick="ApproveVendor" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <style>
        .thumb-container {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
        }

        .thumb {
            position: relative;
            display: inline-block;
        }

            .thumb img {
                width: 120px;
                height: 100px;
                object-fit: cover;
                border-radius: 6px;
                border: 1px solid #ddd;
                cursor: pointer;
                transition: transform 0.2s;
            }

                .thumb img:hover {
                    transform: scale(1.05);
                    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
                }

        .full-view img {
            max-width: 100%;
            max-height: 80vh;
            border-radius: 6px;
            box-shadow: 0 0 10px #000;
        }

        .custom-modal {
            display: none;
            position: fixed;
            z-index: 1050;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.6);
        }

        .custom-modal-content {
            background: #fff;
            margin: 5% auto;
            padding: 20px;
            width: 65%;
            border-radius: 10px;
            box-shadow: 0px 0px 15px rgba(0,0,0,0.4);
            position: relative;
            animation: fadeIn 0.3s ease-in-out;
            text-align: center;
        }

        .close-btn {
            position: absolute;
            top: 12px;
            right: 15px;
            font-size: 24px;
            font-weight: bold;
            cursor: pointer;
            color: #666;
        }

            .close-btn:hover {
                color: #000;
            }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: scale(0.9);
            }

            to {
                opacity: 1;
                transform: scale(1);
            }
        }

        .modal {
            position: fixed !important;
            z-index: 2000 !important;
        }

        .modal-backdrop {
            z-index: 1050 !important;
        }
    </style>
    <script>
        function openUploadPopup(btn) {
            const row = btn.closest("tr");
            const serialNo = row.querySelector("span[id*='lblSerialNo']").innerText.trim();
            const product = row.querySelector("input[id*='txtProduct']")?.value?.trim() || "";

            document.getElementById("popupSerialNo").value = serialNo;
            document.getElementById("popupProduct").value = product;
            document.getElementById("previewContainer").innerHTML = "";

            fetch(`UploadHandler.ashx?action=get&serialNo=${serialNo}&product=${product}`)
                .then(res => res.json())
                .then(data => {
                    if (data.files && data.files.length > 0) {
                        renderThumbnails(data.files);
                    } else {
                        document.getElementById("previewContainer").innerHTML = "<p>No images found.</p>";
                    }
                });

            document.getElementById("imageUploadModal").style.display = "block";
        }

        function closeUploadPopup() {
            document.getElementById("imageUploadModal").style.display = "none";
        }

        function renderThumbnails(files) {
            const container = document.getElementById("previewContainer");
            container.innerHTML = "";

            files.forEach(f => {
                let wrapper = document.createElement("div");
                wrapper.classList.add("thumb");

                let img = document.createElement("img");
                img.src = f.base64 || f;   // supports base64 string or image URL
                img.onclick = () => showFullImage(img.src);

                wrapper.appendChild(img);
                container.appendChild(wrapper);
            });
        }

        function showFullImage(url) {
            const imgElement = document.getElementById("fullImageView");
            imgElement.src = url;
            document.getElementById("fullImageModal").style.display = "block";
        }

        function closeFullImage() {
            document.getElementById("fullImageModal").style.display = "none";
        }

        window.onclick = function (event) {
            if (event.target == document.getElementById("imageUploadModal"))
                closeUploadPopup();
            if (event.target == document.getElementById("fullImageModal"))
                closeFullImage();
        };
        function openApproveModalPage() {
            // clear any previous remarks
            document.getElementById('<%= txtRemarksPage.ClientID %>').value = '';
            // open modal
            var myModal = new bootstrap.Modal(document.getElementById('approveModalPage'));
            myModal.show();
        }
    </script>


</asp:Content>
