<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="LogIn.Pages.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style>
        /* ==== Report Container ==== */
        .report-container {
            background: #fff;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0px 0px 12px rgba(0, 0, 0, 0.1);
            margin: 30px auto;
            width: 90%;
            max-width: 1200px;
            overflow: hidden; /* Keep internal elements aligned */
        }

            .report-container h2 {
                text-align: center;
                margin-bottom: 15px;
                color: #333;
                font-weight: 600;
            }

        /* ==== Filter Panel ==== */
        .filter-panel {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            justify-content: center;
            align-items: flex-end;
            margin-bottom: 25px;
        }

        .filter-item {
            display: flex;
            flex-direction: column;
        }

            .filter-item label {
                font-weight: bold;
                margin-bottom: 5px;
                color: #444;
            }

        .input-control {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            min-width: 180px;
            font-size: 14px;
        }

        /* ==== Buttons ==== */
        .btn {
            padding: 8px 18px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            color: white;
            font-weight: bold;
            transition: 0.3s ease;
        }

        .btn-primary {
            background-color: #007bff;
        }

        .btn-success {
            background-color: #28a745;
        }

        .btn:hover {
            opacity: 0.9;
            transform: translateY(-1px);
        }

        /* ==== GridView Wrapper ==== */
        .grid-container {
            width: 100%;
            overflow-x: auto; /* Enables horizontal scroll if too many columns */
            margin-top: 20px;
            border-radius: 8px;
            border: 1px solid #e1e1e1;
            box-sizing: border-box;
        }

        /* ==== Table Styling ==== */
        table {
            width: 100%;
            min-width: 900px; /* Ensures columns stay readable */
            border-collapse: collapse;
        }

            table th,
            table td {
                border: 1px solid #ddd;
                padding: 8px 12px;
                text-align: center;
                vertical-align: middle;
                font-size: 13px;
                white-space: nowrap; /* Keep content inline */
            }

            table th {
                background-color: #007bff;
                color: white;
                font-weight: 600;
                font-size: 14px;
                text-transform: capitalize;
            }

            table td {
                background-color: #fff;
            }

            table tr:nth-child(even) td {
                background-color: #f8f9fa;
            }

            table tr:hover td {
                background-color: #e9f3ff;
            }

            /* Keep header aligned to left for first column */
            table th:first-child,
            table td:first-child {
                text-align: left;
            }

        /* ==== Responsive Design ==== */
        @media (max-width: 768px) {
            .report-container {
                width: 95%;
                padding: 15px;
            }

            .filter-panel {
                flex-direction: column;
                align-items: stretch;
            }

            .input-control {
                min-width: 100%;
            }

            table {
                min-width: 700px; /* Compact table for small screens */
            }

                table th,
                table td {
                    font-size: 12px;
                    padding: 6px 8px;
                }
        }

        /* ==== Optional Sticky Header ==== */
        table thead th {
            position: sticky;
            top: 0;
            z-index: 2;
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="report-container">
        <h2>📊 Report Viewer</h2>
        <hr />

        <!-- 🔹 Filters Panel -->
        <div class="filter-panel">
            <!-- Report Type -->
            <div class="filter-item">
                <label for="ddlReportType">Report Type:</label>
                <asp:DropDownList ID="ddlReportType" runat="server" CssClass="input-control"></asp:DropDownList>
            </div>

            <!-- From Date -->
            <div class="filter-item">
                <label for="txtFromDate">From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-control" TextMode="Date" />
            </div>

            <!-- To Date -->
            <div class="filter-item">
                <label for="txtToDate">To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="input-control" TextMode="Date" />
            </div>

            <!-- Buttons -->
            <div class="filter-item" style="justify-content: end;">
                <asp:Button ID="btnLoad" runat="server" Text="Load Report" CssClass="btn btn-primary" OnClick="btnLoad_Click" />
                &nbsp;
                <asp:Button ID="btnDownload" runat="server" Text="Download Excel" CssClass="btn btn-success" OnClick="btnDownload_Click" />
            </div>
        </div>

        <hr />

        <!-- 🔹 GridView for Report Data -->
        <div class="grid-container">
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="true" CssClass="gridview"></asp:GridView>
        </div>

    </div>

</asp:Content>
