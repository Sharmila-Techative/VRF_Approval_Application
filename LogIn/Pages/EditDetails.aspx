<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditDetails.aspx.cs" Inherits="LogIn.Pages.EditDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet" />
    <link href="../CSS/Sales.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>


    <style>
        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .modal {
            z-index: 2000 !important;
        }

        .modal-backdrop {
            z-index: 1999 !important;
        }

        .modal-content {
            animation: zoomIn 0.3s ease;
        }

        @keyframes zoomIn {
            from {
                transform: scale(0.8);
                opacity: 0;
            }

            to {
                transform: scale(1);
                opacity: 1;
            }
        }

        /*        .center-header {
            text-align: center;
        }*/
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.6);
        }

        .modal-content {
            background: #fff;
            margin: 10% auto;
            padding: 20px;
            border-radius: 8px;
            width: 400px;
            position: relative;
        }

        .close {
            position: absolute;
            right: 10px;
            top: 10px;
            font-size: 20px;
            cursor: pointer;
        }

        .container {
            display: block;
        }

        #chkRememberOTP {
            pointer-events: none;
        }

        .full-width-flex {
            flex: 1; /* Makes the item take up the full available width */
            background-color: #001f3f;
            padding: 10px;
            width: 100%;
            color: white;
        }

        .full-width-flex1 {
            flex: 1;
            background-color: #001f3f;
            padding: 1px;
            color: white
        }

        .wide-input {
            width: 100%;
            box-sizing: border-box;
            border-top: none;
            border-left: none;
            border-right: none;
        }

        .popup-message {
            position: fixed;
            top: 20px;
            right: 20px;
            padding: 12px 20px;
            border-radius: 6px;
            font-weight: bold;
            color: white;
            z-index: 9999;
            display: none; /* hidden by default */
        }

        .success {
            background-color: #4CAF50; /* Green */
        }

        .error {
            background-color: #f44336; /* Red */
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

        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.6);
            z-index: 100000;
        }

        .modal-content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: #fff;
            padding: 20px;
            width: 600px;
            border-radius: 10px;
            max-height: 80%;
            overflow-y: auto;
        }

        .preview-container {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 10px;
        }

        .preview-item {
            position: relative;
        }

            .preview-item img {
                width: 100px;
                height: 100px;
                object-fit: cover;
                border: 1px solid #ccc;
                border-radius: 5px;
            }

            .preview-item .delete-btn {
                position: absolute;
                top: 0;
                right: 0;
                background: red;
                color: #fff;
                border: none;
                border-radius: 50%;
                width: 20px;
                height: 20px;
                cursor: pointer;
            }

        .readonly-dropdown {
            pointer-events: none; /* Disable mouse clicks */
            background-color: #e9ecef; /* Optional: make it look readonly */
            color: #6c757d;
        }

        #loader {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255,255,255,0.8);
            z-index: 9999; /* loader stays below popup */
            display: none;
            align-items: center;
            justify-content: center;
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
                background: url('../Images/Logo.png') no-repeat center center;
                background-size: 400px; /* adjust watermark size */
                opacity: 0.06; /* subtle watermark effect */
                z-index: 0;
                pointer-events: none; /* let clicks go through */
            }
    </style>



    <script type="text/javascript">
        //history.pushState(null, null, location.href);
        //window.onpopstate = function () {
        //    history.go(1); 
        //window.location.reload(true);

        function disablebackbutton() {
            window.history.forward();
        }
        disablebackbutton();
        window.onload = disablebackbutton;
        window.onpageshow = function (evt) { if (evt.persisted) disablebackbutton(); };
        window.onunload = function () { void (0); };
        //};
        function showLoader() {
            var loader = document.getElementById('loader');
            if (loader) {
                loader.style.display = 'flex';
            }
        }

        // Hide the loader
        function hideLoader() {
            var loader = document.getElementById('loader');
            if (loader) {
                loader.style.display = 'none';
            }
            else loader.style.display = 'none';
        }

        // Show OTP Modal and ensure loader is hidden
        function showOtpModal() {
            hideLoader(); // Hide loader before showing modal
            var otpModal = new bootstrap.Modal(document.getElementById('otpModal'));
            otpModal.show();
        }

        // Show Validate OTP Modal and ensure loader is hidden
        function showValidateModal() {
            hideLoader(); // Hide loader before showing modal
            var validateModal = new bootstrap.Modal(document.getElementById('validateModal'));
            validateModal.show();
        }

        document.addEventListener('DOMContentLoaded', function () {

            // Get modal elements
            var otpModalEl = document.getElementById('otpModal');
            var validateModalEl = document.getElementById('validateModal');

            // Hide loader when modals are closed
            if (otpModalEl) {
                otpModalEl.addEventListener('hidden.bs.modal', function () {
                    hideLoader();
                });
            }
            if (validateModalEl) {
                validateModalEl.addEventListener('hidden.bs.modal', function () {
                    hideLoader();
                });
            }

            // File input change triggers loader
            document.querySelectorAll('input[type="file"]').forEach(function (fileInput) {
                fileInput.addEventListener('change', function () {
                    //if (fileInput.classList.contains('no-loader')) {
                    //    return; // Skip showing loader
                    //}
                    //showLoader();
                    if (fileInput.classList.contains('no-loader')) return;
                    const fileName = this.files.length > 0 ? this.files[0].name.toLowerCase() : "";
                    const validExtensions = [".jpg", ".jpeg", ".png", ".pdf"];
                    const isValid = validExtensions.some(ext => fileName.endsWith(ext));

                    if (isValid) {
                        showLoader();
                    } else {
                        hideLoader(); // Ensure hidden if invalid
                    }
                });
            });

            // Form submit triggers loader unless submitter has 'no-loader' class
            document.querySelectorAll('form').forEach(function (form) {
                form.addEventListener('submit', function (e) {
                    var submitter = e.submitter || document.activeElement;
                    if (submitter && submitter.classList.contains('no-loader')) {
                        return; // Do not show loader
                    }
                    showLoader();
                });
            });

            // Button click triggers loader unless button has 'no-loader' class
            document.querySelectorAll('button, input[type="submit"]').forEach(function (btn) {
                btn.addEventListener('click', function () {
                    if (btn.classList.contains('no-loader')) {
                        return; // Skip showing loader
                    }
                    showLoader();
                });
            });

        });
        function runCopyScripts() {
            var sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice.ClientID %>');
            var sameAsChecked1 = document.getElementById('<%= sameAsRegisteredOffice1.ClientID %>');
            var sameAsChecked2 = document.getElementById('<%= sameAsRegisteredOffice2.ClientID %>');

            if (sameAsChecked && sameAsChecked.checked) {
                copyAddress();
            }
            if (sameAsChecked1 && sameAsChecked1.checked) {
                copyAddress1();
            }
            if (sameAsChecked2 && sameAsChecked2.checked) {
                copyAddress2();
            }
        }

        // Run after the initial page load
        document.addEventListener("DOMContentLoaded", function () {
            runCopyScripts();
        });

        // Run after ASP.NET partial postback
        if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                runCopyScripts();
            });
        }
        // Hide loader when page finishes loading
        window.addEventListener('load', function () {
            hideLoader();
            const sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice.ClientID %>').checked;
            const sameAsChecked1 = document.getElementById('<%= sameAsRegisteredOffice1.ClientID %>').checked;
            const sameAsChecked2 = document.getElementById('<%= sameAsRegisteredOffice2.ClientID %>').checked; s();
            if (sameAsChecked) {
                copyAddress();

            }
            if (sameAsChecked1) {
                copyAddress1();
            }
            if (sameAsChecked2) {
                copyAddress2();
            }
        });
        function uploadFile(input) {
            let arg = input.value; // or extract actual filename
            $.ajax({
                type: "POST",
                url: "YourPage.aspx/HandleFileSelectionAjax",
                data: JSON.stringify({ arg: arg }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    console.log("File selection handled:", res.d);
                }
            });
        }
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
        function OTPPopup1() {

            $("#OTPPopup1").modal('show');

        }


        function CloseOTPPopup() {

            $('.modal-backdrop').remove();

            $('#OTPPopup1').modal('hide');

        }


        function validateFileExtension(input) {
            // hideLoader();
            if (input.files.length > 0) {
                const fileName = input.files[0].name.toLowerCase();
                const validExtensions = [".jpg", ".jpeg", ".png", ".pdf"];
                const isValid = validExtensions.some(ext => fileName.endsWith(ext));

                if (!isValid) {
                    alert("Only .jpg, .jpeg, .png, and .pdf files are allowed.");
                    input.value = ""; // Clear invalid file
                    //showLoader(false);

                    return false;

                }
            }
            return true;
        }

        function saveScrollPosition() {
            var scrollPos = document.documentElement.scrollTop || document.body.scrollTop;
            document.getElementById('<%= HiddenScrollPosition.ClientID %>').value = scrollPos;

        }
        window.onload = function () {
            loadStoredImages();
            var scrollPos = document.getElementById('<%= HiddenScrollPosition.ClientID %>').value;
            if (scrollPos && !isNaN(scrollPos)) {
                window.scrollTo(0, parseInt(scrollPos, 10));
            }
        };


        function addRow() {
            // Get the GridView element by its ClientID
            var gridView = document.getElementById("<%= gvProjectDetails.ClientID %>");
            var rowCount = gridView.rows.length;

            // Create a new row
            var newRow = gridView.insertRow(rowCount);

            // Insert cells in the new row
            var cell1 = newRow.insertCell(0);
            var cell2 = newRow.insertCell(1);
            var cell3 = newRow.insertCell(2);
            var cell4 = newRow.insertCell(3);

            // Create input elements for each cell
            cell1.innerHTML = "<input type='text' style='width: 240px; border-top: none; border-left: none; border-right: none;' name='businessState' />";
            cell2.innerHTML = "<input type='text' style='width: 240px; border-top: none; border-left: none; border-right: none;' name='gstNumber' />";
            cell3.innerHTML = "<input type='text' style='width: 240px; border-top: none; border-left: none; border-right: none;' name='addressOfPlace' />";

            // Create a dropdown for cell4
            cell4.innerHTML = "<select name='gstVendorClassification' style='width: 240px; border-top: none; border-left: none; border-right: none;'>" +
                "<option value='Regular'>Regular</option>" +
                "<option value='Compounding Scheme'>Compounding Scheme</option>" +
                "<option value='PSU/Govt Organisation'>PSU/Govt Organisation</option>" +
                "<option value='Sez'>Sez</option>" +
                "</select>";

            // Reset the input values for the new row
            cell1.firstChild.value = '';
            cell2.firstChild.value = '';
            cell3.firstChild.value = '';
            // Dropdown will initialize with the first option by default
        }

        function downloadImage(imgId) {
            var img = document.getElementById(imgId);
            if (img) {
                var base64Image = img.src;
                var link = document.createElement('a');

                if (base64Image.startsWith('data:application/pdf')) {
                    // Handle PDF download
                    link.href = base64Image;
                    link.download = 'attachment.pdf';
                } else {
                    // Handle image download
                    link.href = base64Image;
                    link.download = 'attachment.png';
                }

                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            } else {
                console.error("Image n found.");
            }
        }
        //function showLoader() {
        //    document.getElementById("loader.style.displa= "flex"; // or block
        //

        //function hideLoader() {
        //    document.getElementById("loader").style.display = "non; // ✅ forcefully remove uploadModa
        // }


        //image upload
        // Open popup for specific row




        // function copyAddress() {
            //const sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice.ClientID %>').checked;

        // if (sameAsChecked) {
                ////document.getElementById('<%= businessBillingAddress1.ClientID %>').value = document.getElementById('<%= registeredOfficeAddress1.ClientID %>').value;
                ////document.getElementById('<%= businessBillingAddress2.ClientID %>').value = document.getElementById('<%= registeredOfficeAddress2.ClientID %>').value;
                ////document.getElementById('<%= businessBillingAddress3.ClientID %>').value = document.getElementById('<%= registeredOfficeAddress3.ClientID %>').value;
                ////document.getElementById('<%= businessBillingCountry.ClientID %>').value = document.getElementById('<%= registeredOfficeCountry.ClientID %>').value;
                ////document.getElementById('<%= businessBillingState.ClientID %>').value = document.getElementById('<%= registeredOfficeState.ClientID %>').value;
              ////  document.getElementById('<%= businessBillingZipCode.ClientID %>').value = document.getElementById('<%= registeredOfficeZipCode.ClientID %>').value;
        //  } else {
                //document.getElementById('<%= businessBillingAddress1.ClientID %>').value = '';
                //document.getElementById('<%= businessBillingAddress2.ClientID %>').value = '';
                //document.getElementById('<%= businessBillingAddress3.ClientID %>').value = '';
                //document.getElementById('<%= businessBillingCountry.ClientID %>').value = '';
                //document.getElementById('<%= businessBillingState.ClientID %>').value = '';
              //  document.getElementById('<%= businessBillingZipCode.ClientID %>').value = '';
        // }
        // }
        function copyAddress() {
            const sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice.ClientID %>').checked;

            const fields = [
                { billing: '<%= businessBillingAddress1.ClientID %>', registered: '<%= registeredOfficeAddress1.ClientID %>' },
                { billing: '<%= businessBillingAddress2.ClientID %>', registered: '<%= registeredOfficeAddress2.ClientID %>' },
                { billing: '<%= businessBillingAddress3.ClientID %>', registered: '<%= registeredOfficeAddress3.ClientID %>' },
                { billing: '<%= businessBillingCity.ClientID %>', registered: '<%= registeredOfficeCity.ClientID %>' },
                { billing: '<%= businessBillingZipCode.ClientID %>', registered: '<%= registeredOfficeZipCode.ClientID %>' }
            ];

            // Country and State dropdowns (separate handling)
            const countryBilling = document.getElementById('<%= businessBillingCountry.ClientID %>');
            const countryRegistered = document.getElementById('<%= registeredOfficeCountry.ClientID %>');

            const stateBilling = document.getElementById('<%= businessBillingState.ClientID %>');
            const stateRegistered = document.getElementById('<%= registeredOfficeState.ClientID %>');

            if (sameAsChecked) {
                // Copy and lock address fields
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    const registeredField = document.getElementById(field.registered);

                    if (billingField && registeredField) {
                        billingField.value = registeredField.value;
                        billingField.readOnly = true;
                    }
                });

                // Copy and disable dropdowns
                if (countryBilling && countryRegistered) {
                    countryBilling.value = countryRegistered.value;
                    //countryBilling.disabled = true;
                    countryBilling.classList.add("readonly-dropdown");
                }

                if (stateBilling && stateRegistered) {
                    stateBilling.value = stateRegistered.value;
                    // stateBilling.disabled = true;
                    stateBilling.classList.add("readonly-dropdown");
                }




            } else {
                // Clear and unlock address fields (but NOT state/country)
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    if (billingField) {
                        billingField.value = '';
                        billingField.readOnly = false;
                    }
                });

                // Enable dropdowns but DO NOT change values
                if (countryBilling) {
                    countryBilling.disabled = false;
                    countryBilling.classList.remove("readonly-dropdown");
                }
                if (stateBilling) {
                    stateBilling.disabled = false;
                    stateBilling.classList.remove("readonly-dropdown");
                }
            }
        }

        function copyAddress1() {
            const sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice1.ClientID %>').checked;

            const fields = [
                { billing: '<%= goodsReturnAddress1.ClientID %>', registered: '<%= registeredOfficeAddress1.ClientID %>' },
                { billing: '<%= goodsReturnAddress2.ClientID %>', registered: '<%= registeredOfficeAddress2.ClientID %>' },
                { billing: '<%= goodsReturnAddress3.ClientID %>', registered: '<%= registeredOfficeAddress3.ClientID %>' },
                { billing: '<%= goodsReturnCity.ClientID %>', registered: '<%= registeredOfficeCity.ClientID %>' },
                { billing: '<%= goodsReturnZipcode.ClientID %>', registered: '<%= registeredOfficeZipCode.ClientID %>' }
            ];

            // Country and State dropdowns (separate handling)
            const countryBilling = document.getElementById('<%= goodsReturnCountry.ClientID %>');
            const countryRegistered = document.getElementById('<%= registeredOfficeCountry.ClientID %>');

            const stateBilling = document.getElementById('<%= goodsReturnState.ClientID %>');
            const stateRegistered = document.getElementById('<%= registeredOfficeState.ClientID %>');

            if (sameAsChecked) {
                // Copy and lock address fields
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    const registeredField = document.getElementById(field.registered);

                    if (billingField && registeredField) {
                        billingField.value = registeredField.value;
                        billingField.readOnly = true;
                    }
                });

                // Copy and disable dropdowns
                if (countryBilling && countryRegistered) {
                    countryBilling.value = countryRegistered.value;
                    //countryBilling.disabled = true;
                    countryBilling.classList.add("readonly-dropdown");
                }

                if (stateBilling && stateRegistered) {
                    stateBilling.value = stateRegistered.value;
                    // stateBilling.disabled = true;
                    stateBilling.classList.add("readonly-dropdown");
                }


            } else {
                // Clear and unlock address fields (but NOT state/country)
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    if (billingField) {
                        billingField.value = '';
                        billingField.readOnly = false;
                    }
                });

                // Enable dropdowns but DO NOT change values
                if (countryBilling) countryBilling.disabled = false;
                if (stateBilling) stateBilling.disabled = false;
            }
        }
        function copyAddress2() {
            const sameAsChecked = document.getElementById('<%= sameAsRegisteredOffice2.ClientID %>').checked;

            const fields = [
                { billing: '<%= shippingAddress1.ClientID %>', registered: '<%= registeredOfficeAddress1.ClientID %>' },
                { billing: '<%= shippingAddress2.ClientID %>', registered: '<%= registeredOfficeAddress2.ClientID %>' },
                { billing: '<%= shippingAddress3.ClientID %>', registered: '<%= registeredOfficeAddress3.ClientID %>' },
                { billing: '<%= shippingCity.ClientID %>', registered: '<%= registeredOfficeCity.ClientID %>' },
                { billing: '<%= shippingZipCode.ClientID %>', registered: '<%= registeredOfficeZipCode.ClientID %>' }
            ];

            // Country and State dropdowns (separate handling)
            const countryBilling = document.getElementById('<%= shippingCountry.ClientID %>');
            const countryRegistered = document.getElementById('<%= registeredOfficeCountry.ClientID %>');

            const stateBilling = document.getElementById('<%= shippingState.ClientID %>');
            const stateRegistered = document.getElementById('<%= registeredOfficeState.ClientID %>');

            if (sameAsChecked) {
                // Copy and lock address fields
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    const registeredField = document.getElementById(field.registered);

                    if (billingField && registeredField) {
                        billingField.value = registeredField.value;
                        billingField.readOnly = true;
                    }
                });

                // Copy and disable dropdowns
                if (countryBilling && countryRegistered) {
                    countryBilling.value = countryRegistered.value;
                    //countryBilling.disabled = true;
                    countryBilling.classList.add("readonly-dropdown");
                }

                if (stateBilling && stateRegistered) {
                    stateBilling.value = stateRegistered.value;
                    //stateBilling.disabled = true;
                    stateBilling.classList.add("readonly-dropdown");
                }


            } else {
                // Clear and unlock address fields (but NOT state/country)
                fields.forEach(field => {
                    const billingField = document.getElementById(field.billing);
                    if (billingField) {
                        billingField.value = '';
                        billingField.readOnly = false;
                    }
                });

                // Enable dropdowns but DO NOT change values
                if (countryBilling) countryBilling.disabled = false;
                if (stateBilling) stateBilling.disabled = false;
            }
        }

        window.addEventListener("DOMContentLoaded", function () {
            const watchFields = [
         '<%= registeredOfficeAddress1.ClientID %>',
         '<%= registeredOfficeAddress2.ClientID %>',
         '<%= registeredOfficeAddress3.ClientID %>',
         '<%= registeredOfficeCity.ClientID %>',
         '<%= registeredOfficeZipCode.ClientID %>',
         '<%= registeredOfficeCountry.ClientID %>',
         '<%= registeredOfficeState.ClientID %>'
            ];

            watchFields.forEach(id => {
                const el = document.getElementById(id);
                if (el) {
                    el.addEventListener("change", function () {
                        if (document.getElementById('<%= sameAsRegisteredOffice.ClientID %>').checked) {
                            copyAddress();
                        }
                    });
                    el.addEventListener("change", function () {
                        if (document.getElementById('<%= sameAsRegisteredOffice1.ClientID %>').checked) {
                            copyAddress1();
                        }
                    });
                    el.addEventListener("input", function () {
                        if (document.getElementById('<%= sameAsRegisteredOffice2.ClientID %>').checked) {
                            copyAddress2();
                        }
                    });
                }
            });
        });
        function removeRow(button) {
            // Remove the row containing the button
            var row = button.parentNode.parentNode;
            row.parentNode.removeChild(row);
        }
        function validateMobileNumber(sender, args) {
            const mobileNumber = args.Value.trim();
            const isValid = /^\d{10}$/.test(mobileNumber); // Regular expression for exactly 10 digits cvMobileNo
            args.IsValid = isValid;
        }
        function validateGSTNumber(sender, args) {
            const gstNumber = args.Value.trim(); // Trim any whitespace
            args.IsValid = gstNumber.length === 15; // Validate length is exactly 15 characters
        }


        function validatePartnerContactNo(inputField) {
            const mobileNumber = inputField.value.trim();

            // Regular expression for exactly 10 digits
            const isValid = /^\d{10}$/.test(mobileNumber);

            if (!isValid) {
                // If invalid, clear the field and show an error in the placeholder
                inputField.value = '';
                inputField.placeholder = "Enter exactly 10 digits!";
                inputField.style.borderColor = "red"; // Optional: Red border for invalid input
            } else {
                // Reset placeholder and border for valid input
                inputField.style.borderColor = "";
            }
        }

        function validatePocContactNo(inputField) {
            const mobileNumber = inputField.value.trim();

            // Regular expression to check if input is exactly 10 digits
            const isValid = /^\d{10}$/.test(mobileNumber);

            if (!isValid) {
                // If invalid, clear the input, change placeholder text, and highlight the field
                inputField.value = '';
                inputField.placeholder = "Enter exactly 10 digits!";
                inputField.style.borderColor = "red";
            } else {
                // Reset styles if the input is valid
                inputField.style.borderColor = "";
            }
        }
        function validateMobileNo(input) {
            const value = input.value.trim();

            // Validate if the input is exactly 10 digits
            const isValid = /^\d{10}$/.test(value);

            if (!isValid) {
                // Clear the input value
                input.value = "";

                // Update the placeholder with an error message
                input.placeholder = "Invalid! Enter a valid 10-digit number";

                // Highlight the field with a red border
                input.style.borderColor = "red";
            } else {
                // Reset placeholder and border color for valid input
                input.placeholder = "Enter 10-digit mobile number";
                input.style.borderColor = "";
            }
        }
        function validateGSTNumber(input) {
            const value = input.value.trim();

            // Check if the input has exactly 15 characters
            if (value.length !== 15) {
                // Clear the invalid value
                input.value = "";

                // Set the placeholder to show an error message
                input.placeholder = "Invalid! Must be 15 characters";

                // Highlight the field with a red border
                input.style.borderColor = "red";
            } else {
                // Reset placeholder and border color if valid
                input.placeholder = "Enter 15-character GST Number";
                input.style.borderColor = "";
            }
        }
        function validateGstNumber(input) {
            const value = input.value.trim();
            const gstRegex = /^[A-Z0-9]{15}$/i;

            if (!gstRegex.test(value)) {
                // Clear invalid input
                input.value = "";

                // Highlight the field with a red border
                input.style.borderColor = "red";

                // Update placeholder with an error message
                input.placeholder = "Invalid! Must be 15 alphanumeric characters.";
            } else {
                // Reset the border and placeholder for valid input
                input.style.borderColor = "";
                input.placeholder = "Enter 15-character GST Number";
            }
        }



    </script>
    <div id="loader" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.8); z-index: 99999; display: none; align-items: center; justify-content: center;">
        <div style="border: 8px solid #f3f3f3; border-top: 8px solid #3498db; border-radius: 50%; width: 60px; height: 60px; animation: spin 1s linear infinite;"></div>
    </div>
    <div class="powered-by" style="position: fixed; bottom: 10px; right: 10px; color: black; font-size: 13px; opacity: 0.7; display: flex; align-items: center; gap: 6px;">
        <p style="margin: 0;"><strong>Powered by</strong></p>
        <img src="../Images/Techative.png" alt="Techative Logo" style="height: 10px;" />
    </div>
    <asp:HiddenField ID="hfPageIndex" runat="server" Value="1" />
    <asp:Panel ID="pnlPage1" runat="server">
        <div class="full-width-flex1">
            <h3 style="margin-left: 340px; font-size: px !important">Business Partner Registration Form</h3>
        </div>
        <asp:Label runat="server" Style="margin-top: 15px;" CssClass="label-container">Contact Person<span style="color:red">*</span></asp:Label>
        <asp:DropDownList
            ID="ContactPerson"
            runat="server"
            Style="width: 200px; margin-top: 15px; margin-bottom: 15px; margin-right: auto; border-left: none; border-right: none;"
            Enabled="true">
        </asp:DropDownList>
        <h5 style="font-size: 16px !important">KYC Documents (Please attach Photocopy)</h5>

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
                                <%# Eval("DocumentType") %>  <span style="color: red;">*</span>
                                <br />
                                <%# Eval("DocumentType").ToString() == "Bank Account" ? "(Cancelled Cheque)" : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Upload" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:FileUpload ID="fileUpload1" runat="server"
                                    onchange='<%# string.Format("if(validateFileExtension(this)) {{ __doPostBack(this.name, \"{0}\"); }}", Eval("DocumentType").ToString()) %>' />
                                <%-- <asp:FileUpload ID="fileUpload1" runat="server" onchange="return validateFileExtension(this);" />--%>


                                <asp:Label ID="lblFileName" runat="server" ForeColor="Green" Font-Italic="true" />

                                <asp:Label ID="DocumentName" runat="server">DocName</asp:Label>
                                <%-- <Span ID="DocumentName" runat="server" ReadOnly="true">Content</Span>--%>
                                <p class="note">
                                    Accepted documents are .jpg, .pdf<br>
                                    <spam style="color: red; font-size: 12px;">File Size should not be more than 2MB</spam>
                                </p>
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
            <asp:Button ID="Button5" runat="server" Text="Previous" OnClick="btnPrevious_Click" Visible="true"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button6" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
            <%-- <asp:Button ID="btnDraft" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 1</strong></p>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlPage2" runat="server">
        <div class="full-width-flex1" style="margin-top: 0px !important;">
            <div class="full-width-flex1" style="text-align: center">
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
                            OnTextChanged="GSTNumber_TextChanged"
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
                        <asp:TextBox ID="PANNumber" runat="server" ClientIDMode="Static" placeholder="PAN Number" ReadOnly="true"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="partnerType">Partner Type<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">
                        <asp:DropDownList ID="ddpartnertype" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;" Enabled="false">
                            <%--<asp:ListItem Text="Select Type" Value="Select Type"></asp:ListItem>--%>
                            <asp:ListItem Text="Vendor" Value="Vendor"></asp:ListItem>
                            <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>


                <tr class="form-row">
                    <td class="label-container">
                        <label for="registeredOfficeAddress1">Registered Office Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="5">
                        <asp:TextBox ID="registeredOfficeAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="registeredOfficeAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="registeredOfficeAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Registered Office Country Dropdown -->
                        <asp:DropDownList ID="registeredOfficeCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList><br>

                        <!-- Registered Office State Dropdown -->

                        <asp:DropDownList ID="registeredOfficeState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList>
                        <asp:TextBox ID="registeredOfficeCity" runat="server" CssClass="full-width" Placeholder="City" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="registeredOfficeZipCode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <%-- <div style="margin-top: 15px;">
                            <asp:CheckBox ID="sameAsRegisteredOffice" runat="server" Text="Same as Registered Office Address" onclick="copyAddress()" />
                        </div>--%>
                    </td>
                    <%-- <td class="label-container">
                        <label for="goodsReturnAddress1" style="visibility:hidden">Goods Return Address<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="5" style="visibility:hidden">
                        <asp:TextBox ID="goodsReturnAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Registered Office Country Dropdown -->
                        <asp:DropDownList ID="goodsReturnCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList><br>

                        <!-- Registered Office State Dropdown -->

                        <asp:DropDownList ID="goodsReturnState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList>
                        <asp:TextBox ID="goodsReturnCity" runat="server" CssClass="full-width" Placeholder="City" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="goodsReturnZipcode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>--%>
                    <td class="label-container">
                        <label for="tradeName" style="visibility: hidden">Trade Name<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" style="visibility: hidden">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="full-width" Style="width: 200px; visibility: hidden border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="businessBillingAddress1">Business / Billing Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="5">
                        <asp:TextBox ID="businessBillingAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="businessBillingAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="businessBillingAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Business Billing Country Dropdown -->

                        <asp:DropDownList ID="businessBillingCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList><br>
                        <!-- Business Billing State Dropdown -->


                        <asp:DropDownList ID="businessBillingState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList>
                        <asp:TextBox ID="businessBillingCity" runat="server" Placeholder="City" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="businessBillingZipCode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <div style="margin-top: 15px;">
                            <asp:CheckBox ID="sameAsRegisteredOffice" runat="server" Text="Same as Registered Office Address" onclick="copyAddress()" />
                        </div>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="goodsReturnAddress1">Goods Return Address<span style="color: red;"></span></label>
                    </td>
                    <td class="input-container" colspan="5">
                        <asp:TextBox ID="goodsReturnAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="goodsReturnAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Registered Office Country Dropdown -->
                        <asp:DropDownList ID="goodsReturnCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList><br>

                        <!-- Registered Office State Dropdown -->

                        <asp:DropDownList ID="goodsReturnState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList>
                        <asp:TextBox ID="goodsReturnCity" runat="server" CssClass="full-width" Placeholder="City" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="goodsReturnZipcode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <%-- <div style="margin-top: 15px;">
                            <asp:CheckBox ID="SameAsReturn" runat="server" Text="Same as Return Address" onclick="copyAddress1()" />
                        </div>--%>
                        <div style="margin-top: 15px;">
                            <asp:CheckBox ID="sameAsRegisteredOffice1" runat="server" Text="Same as Registered Office Address" onclick="copyAddress1()" />
                        </div>
                    </td>
                    <td class="label-container">
                        <label for="tradeName" style="visibility: hidden">Trade Name<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" style="visibility: hidden">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="full-width" Style="width: 200px; visibility: hidden border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="shippingAddress1">Shipping Address<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="5">
                        <asp:TextBox ID="shippingAddress1" runat="server" Placeholder="Address1" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="shippingAddress2" runat="server" Placeholder="Address2" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <asp:TextBox ID="shippingAddress3" runat="server" Placeholder="Address3" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>

                        <!-- Business Billing Country Dropdown -->

                        <asp:DropDownList ID="shippingCountry" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList><br>
                        <!-- Business Billing State Dropdown -->


                        <asp:DropDownList ID="shippingState" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                        </asp:DropDownList>
                        <asp:TextBox ID="shippingCity" runat="server" Placeholder="City" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <asp:TextBox ID="shippingZipCode" runat="server" Placeholder="PinCode" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox><br>
                        <div style="margin-top: 15px;">
                            <asp:CheckBox ID="sameAsRegisteredOffice2" runat="server" Text="Same as Registered Office Address" onclick="copyAddress2()" />
                        </div>
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
                        <label for="dateOfEstablishment">Date of Establishment<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container" colspan="3">
                        <asp:TextBox ID="dateOfEstablishment" runat="server" TextMode="Date" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
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
                        <asp:TextBox ID="emailId" runat="server" TextMode="Email" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                </tr>

                <tr class="form-row">

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
                        <label for="tanNo">TAN Number</label>
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
            <%-- <asp:Button ID="Button13" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
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
                        <asp:DropDownList ID="msmeRegistrationStatus" runat="server" CssClass="full-width" Enabled="false" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                            <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="no"></asp:ListItem>
                            <asp:ListItem Text="Regular" Value="regular"></asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:DropDownList ID="ddlEnterpriseType" runat="server" CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                            <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                            <asp:ListItem Text="Micro" Value="Micro"></asp:ListItem>
                            <asp:ListItem Text="Small" Value="Small"></asp:ListItem>
                            <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:TextBox ID="CreditDays" runat="server"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none; margin-right: 65px !important;">
                        </asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="DisCount">Bill Level Discount</label>
                    </td>
                    <td>
                        <asp:TextBox ID="DisCount" runat="server"
                            CssClass="full-width"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none; margin-right: 56px !important;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="ddlPriceType" style="margin-right: 10px;">Type of Margin</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPriceType" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPriceType_SelectedIndexChanged"
                            Style="width: 200px; border-top: none; border-left: none; border-right: none; margin-right: 157px !important;">
                            <asp:ListItem Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="Markup" Value="Markup"></asp:ListItem>
                            <asp:ListItem Text="Markdown" Value="Markdown"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="label-container">
                        <label id="lblMRPExcel" runat="server" for="btnDownloadExcel" style="margin-right: 13px; visibility: hidden">MRP Calculation Excel</label>
                    </td>
                    <td>
                        <asp:Button ID="btnDownloadExcel" runat="server" Text="Download Excel"
                            class="btn btn-primary no-loader"
                            OnClick="btnDownloadExcel_Click"
                            Style="width: 150px; height: 35px; background-color: #007bff; color: white; border: none; border-radius: 5px; visibility: hidden; margin-right: 83px !important;" />
                    </td>
                    <td></td>
                </tr>
                <asp:Panel ID="pnlMarkdownFields" runat="server" Visible="false">
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
                </asp:Panel>
                <tr class="form-row">
                    <td class="label-container">
                        <label for="BusinessType" style="">Type of  Vendor <span style="color: red;"></span></label>
                    </td>
                    <td class="input-container">
                        <asp:DropDownList ID="BusinessType1" runat="server" CssClass="full-width" AutoPostBack="true"
                            OnSelectedIndexChanged="BusinessType1_SelectedIndexChanged" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                            <asp:ListItem Text="SelectType" Value="SelectType"></asp:ListItem>
                            <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                            <asp:ListItem Text="Agency" Value="Agency"></asp:ListItem>
                            <asp:ListItem Text="SOR" Value="SOR"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td id="Agen1" runat="server" class="label-container" style="visibility: hidden">
                        <label for="AgencyEmail">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="AgencyEmail" runat="server" CssClass="full-width" onblur="forceValidEmail(this)" Style="width: 200px; visibility: hidden; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                        <!-- Required Field Validation -->
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="emailId"
                            ErrorMessage="Email is required."
                            ForeColor="Red" Display="Dynamic"
                            ValidationGroup="vgSave">
                        </asp:RequiredFieldValidator>

                        <!-- Pattern/Format Validation -->
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ControlToValidate="emailId"
                            ErrorMessage="Invalid email format (e.g., name@domain.com)."
                            ForeColor="Red" Display="Dynamic"
                            ValidationGroup="vgSave"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                    </td>

                </tr>
                <tr class="form-row">

                    <td id="Agen2" runat="server" class="label-container" style="visibility: hidden">
                        <label for="AgencyName">Agency Name</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="AgencyName" runat="server" CssClass="full-width" Style="width: 200px; visibility: hidden; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="AgencyEmail" style="visibility: hidden;">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="full-width" onblur="forceValidEmail(this)" Style="width: 200px; border-top: none; border-left: none; border-right: none; visibility: hidden;"></asp:TextBox>
                        <!-- Required Field Validation -->
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="emailId"
                            ErrorMessage="Email is required."
                            ForeColor="Red" Display="Dynamic"
                            ValidationGroup="vgSave">
                        </asp:RequiredFieldValidator>

                        <!-- Pattern/Format Validation -->
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ControlToValidate="emailId"
                            ErrorMessage="Invalid email format (e.g., name@domain.com)."
                            ForeColor="Red" Display="Dynamic"
                            ValidationGroup="vgSave"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                    </td>

                </tr>

                <%--<tr>
                    <td class="label-container">
                        <label for="AgencyName">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="AgencyName" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>
                    <td class="label-container">
                        <label for="AgencyName">Agency Email</label>
                    </td>

                    <td class="input-container">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="full-width" Style="width: 200px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                    </td>

                </tr>--%>
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
                                <asp:FileUpload ID="fileUpload1" runat="server"
                                    onchange='<%# string.Format("if(validateFileExtension(this)) {{ __doPostBack(this.name, \"{0}\"); }}", Eval("DocumentType").ToString()) %>' />
                                <asp:Label ID="lblFileName" runat="server" ForeColor="Green" Font-Italic="true" />
                                <asp:Label ID="DocumentName" runat="server">DocName</asp:Label>
                                <p class="note">
                                    Accepted documents are .jpg, .pdf<br />
                                    <span style="color: red; font-size: 12px;">File Size should not be more than 2MB</span>
                                </p>
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

                                <asp:LinkButton ID="lnkDeleteRow4" runat="server"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    OnClick="lnkDelete_Click4" ForeColor="Red" ToolTip="Remove Row">
                            <i class="fa fa-close" style="margin-left:13px;"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="lnkAddRow4" runat="server" ForeColor="Green"
                                    OnClick="lnknewrowadd_Click4"
                                    OnClientClick="saveScrollPosition(); setTimeout(restoreScrollPosition, 0);">
                            <img src="../Images/PlusIcon.png" style="width:18px; margin-left:13px;" />
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
            <%-- <asp:Button ID="Button15" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
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
                        <strong>Bank Account Details span<span style="color: red;">(Mandatory)</span></strong>
                    </td>
                </tr>

                <tr class="form-row">
                    <td class="label-container">
                        <label for="bankName">Name of Bank:<span style="color: red;">*</span></label>
                    </td>
                    <td class="input-container">

                        <asp:DropDownList ID="bankName" runat="server" CssClass="full-width"
                            Style="width: 255px; border-top: none; border-left: none; border-right: none; margin-right: 0px; float: right;">
                        </asp:DropDownList>

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
                        <asp:TextBox ID="bankAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="full-width" Style="width: 255px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="full-width-flex11">
            <%-- <h3 style="margin-left: 340px;">Business Partner Registration Form</h3>--%>
        </div>

        <div style="margin-left: 20px">
            <h5>Other business location (Optional)</h5>

            <%--  <div class="sal-grid">
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
                                    <asp:DropDownList ID="businessState" runat="server" CssClass="full-width"
                                        Style="width: 205px; border-top: none; border-left: none; border-right: none;">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GST Number" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
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
                                    <asp:DropDownList ID="gstVendorClassification" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                                        <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                                        <asp:ListItem Text="Compounding Scheme" Value="Compounding Scheme"></asp:ListItem>
                                        <asp:ListItem Text="PSU/Govt Organisation" Value="PSU/Govt Organisation"></asp:ListItem>
                                        <asp:ListItem Text="Sez" Value="Sez"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkgrddelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server" OnClick="lnkDelete_Click" ForeColor="Red">
                                    <i class="fa fa-close"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkaddRow_Click" runat="server" ForeColor="Green" OnClick="lnknewrowadd_Click" OnClientClick="saveScrollPosition()">
<img src="../Images/PlusIcon.png" style="width:18px; margin-left:13px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:HiddenField ID="HiddenScrollPosition" runat="server" />
                </div>
            </div>--%>
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
                                    <asp:DropDownList ID="businessState" runat="server" CssClass="full-width"
                                        Style="width: 205px; border-top: none; border-left: none; border-right: none;">
                                    </asp:DropDownList>
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
                                        onkeypress="allowGstCharacters(event)"
                                        onpaste="handleGstPaste(event)"
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
                                    <asp:DropDownList ID="gstVendorClassification" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none;">
                                        <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                                        <asp:ListItem Text="Compounding Scheme" Value="Compounding Scheme"></asp:ListItem>
                                        <asp:ListItem Text="PSU/Govt Organisation" Value="PSU/Govt Organisation"></asp:ListItem>
                                        <asp:ListItem Text="Sez" Value="Sez"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkgrddelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server" OnClick="lnkDelete_Click" ForeColor="Red">
                                    <i class="fa fa-close"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkaddRow_Click" runat="server" ForeColor="Green" OnClick="lnknewrowadd_Click" OnClientClick="saveScrollPosition()">
<img src="../Images/PlusIcon.png" style="width:18px; margin-left:13px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:HiddenField ID="HiddenScrollPosition" runat="server" />
                </div>
            </div>




            <h5>Partners/Proprietor/Director Detail (Provide at Least One Person Details)</h5>

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
                                    <asp:TextBox ID="partnerEmail" runat="server" TextMode="Email" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="center-header">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkgrddelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server" OnClick="lnkDelete_Click1" ForeColor="Red">
                                    <i class="fa fa-close"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="Green" OnClick="lnknewrowadd_Click1" OnClientClick="saveScrollPosition(); setTimeout(restoreScrollPosition, 0);">
                                    <img src="../Images/PlusIcon.png"  style="width:18px; margin-left:13px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:HiddenField ID="HiddenField4" runat="server" />

                </div>
            </div>
        </div>
        <asp:Panel ID="Panel3" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button3" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button4" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
            <%-- <asp:Button ID="Button16" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 4</strong></p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlPage5" runat="server" Visible="false">

        <h5>Primary Operational Contacts</h5>

        <div class="sal-grid">
            <div style="overflow: auto; overflow-x: hidden;">

                <asp:GridView ID="gvOperationalContacts" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvOperationalContacts_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="center-header" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="pocName" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="center-header" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="pocDesignation" runat="server" Style="width: 230px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact No." HeaderStyle-CssClass="center-header">
                            <ItemTemplate>

                                <asp:TextBox
                                    ID="pocContactNo"
                                    runat="server"
                                    Style="width: 250px; border-top: none; border-left: none; border-right: none;"
                                    onblur="validatePocContactNo(this)"
                                    placeholder="Enter 10-digit mobile number">
                                </asp:TextBox>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email-ID" HeaderStyle-CssClass="center-header">
                            <ItemTemplate>
                                <asp:TextBox ID="pocEmail" runat="server" TextMode="Email" Style="width: 340px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


            </div>
        </div>

        <h5>Major goods and services Details  </h5>

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
                        <asp:TemplateField HeaderText="Product" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProduct" runat="server" Style="width: 240px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Brand" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBrand" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Size" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSize" runat="server" Style="width: 200px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Material Description" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMaterialDescription" runat="server" Style="width: 140px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HSN Code" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtHSNCode" runat="server" Style="width: 100px; border-top: none; border-left: none; border-right: none"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax %" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTaxPercentage" runat="server" TextMode="Number" Style="width: 90px; border-top: none; border-left: none; border-right: none;"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ImageUpload">
                            <ItemTemplate>
                                <%-- <input type="button" value="Upload"
                                    onclick="openUploadPopup(this);"
                                    class="btn btn-primary" />
                                <asp:HiddenField ID="HiddenFieldProductId" runat="server" CssClass="no-loader"
                                    Value='<%# Eval("Product") %>' />--%>
                                <input type="button" value="Upload" class="btn btn-primary no-loader" onclick="openUploadPopup(this)" />


                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="center-header" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkgrddelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server" OnClick="lnkDelete_Click2" ForeColor="Red">
<i class="fa fa-close"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" runat="server" ForeColor="Green" OnClick="lnknewrowadd_Click2" OnClientClick="saveScrollPosition(); setTimeout(restoreScrollPosition, 0);">
<img src="../Images/PlusIcon.png"  style="width:18px; margin-left:13px;" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:HiddenField ID="HiddenField3" runat="server" />

            </div>
        </div>

        <asp:Panel ID="Panel4" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="Button9" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="Button10" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
            <%--  <asp:Button ID="Button17" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
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


                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="center-header">
                    <ItemStyle Width="15%" />
                    <HeaderStyle Width="15%" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkgrddelete" CommandArgument='<%# Container.DataItemIndex %>' runat="server"
                            OnClick="lnkDelete_Click3" ForeColor="Red">
                    <i class="fa fa-close" style=" margin-left:53px;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" ForeColor="Green"
                            OnClick="lnknewrowadd_Click3" OnClientClick="saveScrollPosition(); setTimeout(restoreScrollPosition, 0);">
                    <img src="../Images/PlusIcon.png" style="width:18px; margin-left:13px;" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>


        <h5>Other Information</h5>
        <div class="sal-grid">
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
            <%-- <asp:Button ID="Button18" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
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

                <asp:TextBox ID="declarationDesignation" runat="server" Style="margin-left: 37px"></asp:TextBox>
            </p>
            <p>
                Mobile No:<span style="color: red;">*</span>

                <asp:TextBox ID="MobileNo1" runat="server" Style="margin-left: 50px"></asp:TextBox>

                <%-- <asp:Button ID="VerifyOtp" runat="server" CssClass="btn submit-btn" Text="Send OTP" OnClick="SendOTPBtn_Click" />
                <asp:TextBox ID="OTP" runat="server" Style="margin-left: 61px" placeholder="Enter OTP"></asp:TextBox>--%>
                <%-- <asp:Button ID="Button14" runat="server" CssClass="btn submit-btn no-loader" Text="Validate OTP" OnClick="ValidateOTP" Style="width: 170px; height: 29px; font-size: 13px; text-align: center;" />--%>
                <%-- <asp:CheckBox ID="chkRememberOTP" runat="server" Text="Is Valid" Enabled="false"
                    Style="margin-left: 10px; vertical-align: middle;" />--%>
            </p>
            <%--   <p id="otpMessage" runat="server" style="color: green; margin-left: 350px; display: none;">OTP Valid for 15 Mins</p>--%>
        </div>
        <%--<asp:Panel ID="pnlOtpMessage" runat="server" CssClass="popup-message" Visible="false">
            <asp:Label ID="lblOtpMessage" runat="server" Text=""></asp:Label>
        </asp:Panel>--%>
        <!-- OTP Sent Modal -->
        <!-- OTP Sent Modal -->
        <div class="modal fade" id="otpModal" runat="server" clientidmode="Static" tabindex="-1" aria-labelledby="otpModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content shadow-lg border-0 rounded-4">
                    <div class="modal-header bg-primary text-white rounded-top-4">
                        <h5 class="modal-title" id="otpModalLabel">OTP Sent</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body text-center py-4">
                        <i class="bi bi-check-circle-fill text-success fs-2 mb-3"></i>
                        <p class="mb-0 fw-semibold">OTP has been sent successfully!</p>
                    </div>
                    <div class="modal-footer justify-content-center border-0 pb-4">
                        <button type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Validate OTP Modal -->
        <div class="modal fade" id="validateModal" runat="server" clientidmode="Static" tabindex="-1" aria-labelledby="validateModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content shadow-lg border-0 rounded-4">
                    <div class="modal-header bg-success text-white rounded-top-4">
                        <h5 class="modal-title" id="validateModalLabel">OTP Validated</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body text-center py-4">
                        <i class="bi bi-shield-check text-success fs-2 mb-3"></i>
                        <p class="mb-0 fw-semibold">OTP has been validated successfully!</p>
                    </div>
                    <div class="modal-footer justify-content-center border-0 pb-4">
                        <button type="button" class="btn btn-success px-4" data-bs-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- Upload Modal: place outside GridView -->

        <div class="button-container" style="margin-left: 20px">
            <asp:Button ID="SubmitButton" runat="server" CssClass="btn submit-btn" Text="Submit" OnClick="SubmitButton_Click" />
            <asp:Button ID="CancelButton" runat="server" CssClass="btn cancel-btn" Text="Cancel" OnClick="CancelButton_Click" />
            <%--<asp:Button ID="Draft" runat="server" CssClass="btn Draft-btn" Text="Draft" OnClick="DraftButton_Click" Style="width: 100px !important; background-color: #8B8C89" />--%>
            <%-- <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="nav-button submit-btn" OnClick="btnPreview_Click" />--%>
        </div>

        <asp:Panel ID="pnlNavigation" runat="server" Style="margin-top: 10px; text-align: right;">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" OnClick="btnPrevious_Click"
                CssClass="nav-button prev-button" />
            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click"
                CssClass="nav-button next-button" />
            <%-- <asp:Button ID="Button19" runat="server" Text="Draft" CssClass="btn btn-warning"
                OnClick="DraftButton_Click" />--%>
        </asp:Panel>
        <div style="position: fixed; bottom: 10px; left: 20px; color: black; font-size: 13px; opacity: 0.7;">
            <p><strong>Page: 7</strong></p>
        </div>
    </asp:Panel>

    <!-- Image Upload Modal -->
    <div id="imageUploadModal" class="custom-modal">
        <div class="custom-modal-content">
            <span class="close-btn" onclick="closeUploadPopup()">&times;</span>
            <h3 style="margin-bottom: 15px;">Upload Images</h3>

            <input type="hidden" id="popupSerialNo" />
            <input type="hidden" id="popupProduct" />

            <div class="upload-box">
                <input type="file" id="fileInput" multiple class="no-loader" />
                <button class="upload-btn no-loader" onclick="uploadImages()">Upload</button>
            </div>

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

    <style>
        /* Generic modal */
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
        }

        .full-view {
            max-width: 90%;
            max-height: 90%;
            text-align: center;
        }

            .full-view img {
                max-width: 100%;
                max-height: 80vh;
                border-radius: 6px;
                box-shadow: 0 0 10px #000;
            }

        /* Close button */
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

        /* Upload area */
        .upload-box {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 15px;
        }

        .upload-btn {
            padding: 6px 14px;
            background: #007bff;
            border: none;
            border-radius: 4px;
            color: #fff;
            cursor: pointer;
            transition: 0.2s;
        }

            .upload-btn:hover {
                background: #0056b3;
            }

        /* Thumbnails */
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

        .delete-btn {
            position: absolute;
            top: -6px;
            right: -6px;
            border: none;
            background: red;
            color: white;
            font-size: 14px;
            cursor: pointer;
            border-radius: 50%;
            width: 22px;
            height: 22px;
            line-height: 18px;
            text-align: center;
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
    </style>

    <script>
        function openUploadPopup(btn) {
            const row = btn.closest("tr");
            const serialNo = row.querySelector("span[id*='lblSerialNo']").innerText.trim();
            const product = row.querySelector("input[id*='txtProduct']").value.trim();

            document.getElementById("popupSerialNo").value = serialNo;
            document.getElementById("popupProduct").value = product;
            document.getElementById("fileInput").value = "";
            document.getElementById("previewContainer").innerHTML = "";

            // Load existing images
            fetch(`UploadHandler.ashx?action=get&serialNo=${serialNo}&product=${product}`)
                .then(res => res.json())
                .then(data => {
                    if (data.files) renderThumbnails(data.files);
                });

            document.getElementById("imageUploadModal").style.display = "block";
        }
        function closeUploadPopup() {
            document.getElementById("imageUploadModal").style.display = "none";
        }
        function showFullImage(url) {
            document.getElementById("fullImageView").src = url;
            document.getElementById("fullImageModal").style.display = "block";
        }
        function closeFullImage() {
            document.getElementById("fullImageModal").style.display = "none";
        }
        function uploadImages() {
            const files = document.getElementById("fileInput").files;
            if (!files.length) return alert("Select images first");

            const serialNo = document.getElementById("popupSerialNo").value;
            const product = document.getElementById("popupProduct").value;

            const formData = new FormData();
            Array.from(files).forEach(file => formData.append("files", file));
            formData.append("serialNo", serialNo);
            formData.append("product", product);

            fetch("UploadHandler.ashx", { method: "POST", body: formData })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        renderThumbnails(data.files);
                    } else {
                        alert("Upload failed");
                    }
                })
                .catch(err => console.error(err));
        }
        function renderThumbnails(files) {
            const container = document.getElementById("previewContainer");
            container.innerHTML = "";

            files.forEach(f => {
                let wrapper = document.createElement("div");
                wrapper.classList.add("thumb");

                let img = document.createElement("img");
                img.src = f.base64 || f;   // supports base64 or URL
                img.onclick = () => showFullImage(img.src);

                let delBtn = document.createElement("button");
                delBtn.innerHTML = "×";
                delBtn.classList.add("delete-btn");

                delBtn.onclick = function (e) {
                    e.stopPropagation();
                    if (confirm("Delete this image?")) {
                        fetch("UploadHandler.ashx?action=delete&file=" + encodeURIComponent(f.fileName))
                            .then(r => r.json())
                            .then(res => {
                                if (res.success) wrapper.remove();
                                else alert("Delete failed: " + res.message);
                            });
                    }
                };

                wrapper.appendChild(img);
                wrapper.appendChild(delBtn);
                container.appendChild(wrapper);
            });
        }
        // Close modal on outside click
        window.onclick = function (event) {
            if (event.target == document.getElementById("imageUploadModal"))
                closeUploadPopup();
            if (event.target == document.getElementById("fullImageModal"))
                closeFullImage();
        };
    </script>
</asp:Content>
