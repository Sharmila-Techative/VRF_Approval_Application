<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="LogIn.Pages.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>



    <div class="master-page">
        <div class="heading">
            <a href="UserList.aspx">
                <i class='bx bx-chevrons-left' title="Go To Dashboard"></i>
            </a>
            <h2>Create User</h2>
            <hr />
        </div>
        <div class="master-form">

            <!-- User Name -->
            <div class="master-group">
                <label class="label">User Id <span class="span1">*</span> </label>
                <asp:TextBox ID="txtUserId" runat="server" CssClass="master-panel" Enabled="false"></asp:TextBox>
            </div>

            <!-- User Name -->
            <div class="master-group">
                <label class="label">User Name <span class="span1">*</span> </label>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="master-panel" placeholder="Enter User Name"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator>
                <asp:Label CssClass="errormsg" ID="Invaliduserlabel" runat="server" ForeColor="Red" Visible="False" Display="Dynamic"></asp:Label>
            </div>





            <!-- Password -->
            <div class="master-group">
                <label class="label">Password <span class="span1">*</span></label>
                <div class="password-container">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="master-panel" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                    <span class="password-toggle-icon" data-target="#txtPassword">
                        <i class="fas fa-eye-slash"></i>
                    </span>
                </div>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator>
            </div>

            <!-- Confirm Password -->
            <div class="master-group">
                <label class="label">Confirm Password <span class="span1">*</span></label>
                <div class="password-container">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="master-panel" TextMode="Password" placeholder="Re-Enter Password"></asp:TextBox>
                    <span class="password-toggle-icon" data-target="#txtConfirmPassword">
                        <i class="fas fa-eye-slash"></i>
                    </span>
                </div>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtConfirmPassword" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator>
                <asp:CompareValidator CssClass="errormsg" ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ForeColor="Red" Display="Dynamic" ErrorMessage="Password does not match."></asp:CompareValidator>
            </div>

            <!-- User Mail Id -->
            <div class="master-group">
                <label class="label">Mail Id <span class="span1">*</span> </label>
                <asp:TextBox ID="txtUserMail" runat="server" CssClass="master-panel" TextMode="Email" placeholder="Enter E-mail"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtUserMail" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator CssClass="errormsg" ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserMail" ForeColor="Red" Display="Dynamic" ErrorMessage="Enter valid E-mail" ValidationGroup="valGroup" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"></asp:RegularExpressionValidator>
                <asp:Label CssClass="errormsg" ID="Invalidmaillabel" runat="server" ForeColor="Red" Visible="False" Display="Dynamic"></asp:Label>
            </div>

            <!-- Mobile No -->
            <div class="master-group">
                <label class="label">Mobile No<span class="span1">*</span></label>
                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="master-panel" TextMode="Phone" placeholder="Enter Contact No"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobileNo" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator CssClass="errormsg" ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMobileNo" ErrorMessage="Enter valid mobile-number" ValidationGroup="valGroup" ForeColor="Red" Display="Dynamic" ValidationExpression="[0-9]{10}" />
            </div>
            <%-- <div class="master-group">
                <label class="label">Department<span class="span1">*</span></label>
                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="master-panel"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator7" runat="server"
                    ControlToValidate="ddlDepartment" InitialValue="0" ForeColor="Red" Display="Dynamic"
                    ErrorMessage="Field Required."></asp:RequiredFieldValidator>
                <asp:Label CssClass="errormsg" ID="Label1" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </div>--%>
            <!-- Department Section -->
            <div class="master-group">
                <label class="label">Department<span class="span1">*</span></label>
                <div style="display: flex; align-items: center; width: 253px;">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="master-panel" Style="flex: 1;"></asp:DropDownList>
                    <%--<button type="button" class="btn-add" onclick="openDeptModal()" title="Add / Manage Department">
                        <i class="fa fa-plus"></i>
                    </button>--%>
                </div>
                <asp:Label CssClass="errormsg" ID="Invalidselect" runat="server" ForeColor="Red" Visible="False" Display="Dynamic"></asp:Label>
            </div>
            <div class="form-group mb-3">
                <label for="txtCount">Level<span class="span1">*</span></label>
                <div class="input-group">
                    <button type="button" class="btn btn-outline-secondary" onclick="decrementCount()">−</button>
                    <asp:TextBox ID="txtCount" runat="server" CssClass="form-control text-center" Text="1"></asp:TextBox>
                    <button type="button" class="btn btn-outline-secondary" onclick="incrementCount()">+</button>
                </div>
            </div>
            <!-- Department Modal -->
            <div id="deptModal" class="custom-modal">
                <div class="custom-modal-content">
                    <span class="close-btn" onclick="closeDeptModal()">&times;</span>
                    <h3>Manage Departments</h3>

                    <div class="modal-section">
                        <input type="text" id="txtNewDept" placeholder="Enter Department Name" class="form-control" />
                        <button type="button" class="btn-success" onclick="addDepartment()">Add</button>
                    </div>

                    <hr />
                    <h4>Existing Departments</h4>
                    <div id="deptList"></div>
                </div>
            </div>
            <script>
                function openDeptModal() {
                    document.getElementById('deptModal').style.display = 'block';
                    loadDepartments();
                }

                function closeDeptModal() {
                    document.getElementById('deptModal').style.display = 'none';
                }

                window.onclick = function (event) {
                    if (event.target == document.getElementById('deptModal')) closeDeptModal();
                }

                // Load Departments dynamically
                function loadDepartments() {
                    $.ajax({
                        type: "POST",
                        url: "User.aspx/GetAllDepartments",
                        data: '{}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            let data = response.d;
                            let html = "<ul>";
                            data.forEach(function (d) {
                                html += `<li>${d.DepartmentName} <button onclick="deleteDepartment('${d.DepartmentCode}')"><i class="fa fa-trash"></i></button></li>`;
                            });
                            html += "</ul>";
                            $('#deptList').html(html);
                        }
                    });
                }

                // Add Department dynamically
                function addDepartment() {
                    let name = $('#txtNewDept').val().trim();
                    if (name === "") { alert("Enter department name."); return; }

                    $.ajax({
                        type: "POST",
                        url: "User.aspx/AddDepartment",
                        data: JSON.stringify({ deptName: name }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "success") {
                                $('#txtNewDept').val('');
                                loadDepartments();
                                refreshDropdown();
                            } else {
                                alert(response.d);
                            }
                        }
                    });
                }

                // Delete Department dynamically
                function deleteDepartment(code) {
                    if (!confirm("Are you sure to delete this department?")) return;

                    $.ajax({
                        type: "POST",
                        url: "User.aspx/DeleteDepartment",
                        data: JSON.stringify({ deptCode: code }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "success") {
                                loadDepartments();
                                refreshDropdown();
                            } else {
                                alert(response.d);
                            }
                        }
                    });
                }

                // Refresh dropdown after add/delete
                function refreshDropdown() {
                    $.ajax({
                        type: "POST",
                        url: "User.aspx/GetAllDepartments",
                        data: '{}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            let ddl = $('#<%= ddlDepartment.ClientID %>');
                            ddl.empty();
                            ddl.append(`<option value="0">-- Select Department --</option>`);
                            response.d.forEach(function (d) {
                                ddl.append(`<option value="${d.DepartmentCode}">${d.DepartmentName}</option>`);
                            });
                        }
                    });
                }
            </script>
            <script>
                function incrementCount() {
                    var txt = document.getElementById('<%= txtCount.ClientID %>');
                    var val = parseInt(txt.value || "0");
                    txt.value = val + 1;
                }

                function decrementCount() {
                    var txt = document.getElementById('<%= txtCount.ClientID %>');
                    var val = parseInt(txt.value || "0");
                    if (val > 1) txt.value = val - 1;
                }
            </script>
            <style>
                .custom-modal {
                    display: none;
                    position: fixed;
                    z-index: 1000;
                    left: 0;
                    top: 0;
                    width: 100%;
                    height: 100%;
                    background-color: rgba(0,0,0,0.5);
                }

                .custom-modal-content {
                    background-color: #fff;
                    margin: 10% auto;
                    padding: 20px;
                    border-radius: 10px;
                    width: 400px;
                    position: relative;
                    box-shadow: 0 5px 15px rgba(0,0,0,0.3);
                }

                .close-btn {
                    position: absolute;
                    top: 10px;
                    right: 15px;
                    font-size: 24px;
                    font-weight: bold;
                    cursor: pointer;
                }

                .btn-add {
                    background-color: #007bff;
                    color: white;
                    border: none;
                    padding: 6px 12px;
                    cursor: pointer;
                    border-radius: 4px;
                    margin-left: 5px;
                }

                    .btn-add:hover {
                        background-color: #0056b3;
                    }

                .btn-success {
                    background-color: #28a745;
                    color: white;
                    border: none;
                    padding: 6px 12px;
                    cursor: pointer;
                    border-radius: 4px;
                    margin-top: 5px;
                }

                    .btn-success:hover {
                        background-color: #1e7e34;
                    }

                .dept-item {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    padding: 6px 10px;
                    border: 1px solid #ccc;
                    border-radius: 4px;
                    margin-bottom: 5px;
                }

                .btn-delete {
                    background-color: #dc3545;
                    color: white;
                    border: none;
                    padding: 4px 8px;
                    border-radius: 4px;
                    cursor: pointer;
                }

                    .btn-delete:hover {
                        background-color: #a71d2a;
                    }
            </style>





            <br />

            <!-- Toggles -->
            <div class="toggle-group">
                <div class="master-group">
                    <label class="label">Active</label>
                    <label class="switch">
                        <asp:CheckBox ID="chkact" runat="server" /><span class="slider round"></span></label>
                </div>

            </div>

            <br />
            <!-- Attachments -->
            <div class="master-group">
                <label class="label">Profile Upload <span class="span1"></span></label>
                <div class="attachment-container">
                    <asp:FileUpload ID="fileUpload" runat="server" multiple="multiple" CssClass="master-panel" accept=".jpg, .jpeg, .png,.jfif" />

                    <asp:CustomValidator ID="CustomValidator1" ForeColor="Red" runat="server" ControlToValidate="fileUpload"
                        Display="Dynamic"></asp:CustomValidator>

                    <div class="image-wrapper" onclick="openModal();">
                        <asp:Label ID="lblAttachment" runat="server" CssClass="attachment-label" />
                        <asp:Image ID="imgAttachment" ToolTip="Click to View" runat="server" CssClass="attachment-image" Visible="false" />
                    </div>

                    <asp:LinkButton ID="btnViewAttachments" runat="server" title="Click to view attachment" CssClass="icon-btn" OnClick="btnViewAttachments_Click">
                        <i class="bx bx-link" title="View Existing Attachment"></i>
                    </asp:LinkButton>
                </div>
            </div>

            <!-- Modal Structure for viewing the image -->
            <div id="imageModal" class="modal" style="display: none;">
                <span class="close" onclick="closeModal()">&times;</span>
                <img class="modal-content" id="imgInModal" />
                <div id="caption"></div>
            </div>
            <br />

            <!-- Buttons -->
            <div>
                <asp:LinkButton ID="btnSave" ToolTip="click for Submit" CssClass="link-button" runat="server" type="button" OnClientClick="if(!validateDepartment()) return false; return confirm('Click OK to submit the form or Cancel to go back.');" OnClick="btnSave_Click">SUBMIT</asp:LinkButton>
                <asp:LinkButton ID="btnClear" ToolTip="Click for Clear fields" CssClass="link-button" runat="server" type="button" OnClick="btnClear_Click" CausesValidation="false">&nbsp;CLEAR&nbsp;</asp:LinkButton>

                <asp:LinkButton CssClass="link-button" ID="update" runat="server" Visible="false" OnClientClick="return confirm('Click OK to Update the form or Cancel to go back.');" OnClick="update_Click">UPDATE</asp:LinkButton>
                <asp:LinkButton CssClass="link-button" ID="erase" runat="server" CausesValidation="false" Visible="false" OnClick="erase_Click">CANCEL</asp:LinkButton>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function validateDepartment() {
            var ddl = document.getElementById('<%= ddlDepartment.ClientID %>');
            var errorLabel = document.getElementById('<%= Invalidselect.ClientID %>');

            if (ddl.value === "0" || ddl.value === "") { // assuming "0" is default
                errorLabel.style.display = "inline";  // show error
                return false; // prevent submit
            } else {
                errorLabel.style.display = "none";   // hide error
                return true; // allow submit
            }
        }

        // Hide error when selection changes
        $('#<%= ddlDepartment.ClientID %>').change(function () {
            var errorLabel = $('#<%= Invalidselect.ClientID %>');
            if ($(this).val() !== "0" && $(this).val() !== "") {
                errorLabel.hide();
            }
        });

        // Open the modal
        function openModal() {
            var modal = document.getElementById('imageModal');
            var modalImg = document.getElementById('imgInModal');
            var img = document.getElementById('<%= imgAttachment.ClientID %>');

            if (img.src) { // Check if the image source is not empty
                modal.style.display = "block";
                modalImg.src = img.src; // Set the modal image source to the clicked image's source
                document.getElementById("caption").innerHTML = document.getElementById('<%= lblAttachment.ClientID %>').innerHTML;
            }
        }
        function closeModal() {
            document.getElementById('imageModal').style.display = "none";
        }

        $("#IsEnabled").change(function () {
            if ($("#IsEnabled").is(":checked")) {
            }
        });

        document.querySelectorAll('.password-toggle-icon').forEach(icon => {
            icon.addEventListener('click', function () {
                const input = this.previousElementSibling;
                if (input.type === 'password') {
                    input.type = 'text';
                    this.innerHTML = '<i class="fas fa-eye"></i>';
                } else {
                    input.type = 'password';
                    this.innerHTML = '<i class="fas fa-eye-slash"></i>';
                }
            });
        });
    </script>
</asp:Content>
