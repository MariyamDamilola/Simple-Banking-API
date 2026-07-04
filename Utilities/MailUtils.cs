namespace SimpleBankingAPI.Utilities;

public static class MailUtils
{
    private const string BrandColor = "#BF092F";
    private const string BrandColorDark = "#8C0722";
    private const string BankName = "Mighty Bank";

    public static string GetEmailWrapper(string preheader, string title, string content)
    {
        var year = DateTime.UtcNow.Year;
        return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1.0' />
<title>{title}</title>
</head>
<body style='margin:0; padding:0; background-color:#f2f2f5; font-family: Helvetica, Arial, sans-serif;'>

    <!-- Preheader (hidden preview text) -->
    <div style='display:none; max-height:0; overflow:hidden; opacity:0;'>
        {preheader}
    </div>

    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#f2f2f5; padding: 30px 0;'>
        <tr>
            <td align='center'>
                <table role='presentation' width='600' cellpadding='0' cellspacing='0' style='width:600px; max-width:600px; background-color:#ffffff; border-radius:10px; overflow:hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.06);'>

                    <!-- Top accent bar -->
                    <tr>
                        <td style='background-color:{BrandColor}; height:6px; line-height:6px; font-size:0;'>&nbsp;</td>
                    </tr>

                    <!-- Header / Logo -->
                    <tr>
                        <td style='background-color:#ffffff; padding:36px 40px 24px 40px; text-align:center; border-bottom: 1px solid #f0f0f0;'>
                            <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:26px; font-weight:700; color:{BrandColor}; letter-spacing:0.5px;'>
                                {BankName}
                            </div>
                            <div style='font-family: Helvetica, Arial, sans-serif; font-size:11px; letter-spacing:2px; text-transform:uppercase; color:#9a9a9a; margin-top:4px;'>
                                Secure &middot; Reliable &middot; Yours
                            </div>
                        </td>
                    </tr>

                    <!-- Title bar -->
                    <tr>
                        <td style='background-color:{BrandColorDark}; padding:18px 40px;'>
                            <span style='font-family: Helvetica, Arial, sans-serif; color:#ffffff; font-size:16px; font-weight:600; letter-spacing:0.3px;'>{title}</span>
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style='padding:40px; font-family: Helvetica, Arial, sans-serif; color:#333333; font-size:15px; line-height:1.65;'>
                            {content}
                        </td>
                    </tr>

                    <!-- Divider -->
                    <tr>
                        <td style='padding:0 40px;'>
                            <div style='border-top:1px solid #eeeeee;'></div>
                        </td>
                    </tr>

                    <!-- Support strip -->
                    <tr>
                        <td style='padding:24px 40px; text-align:center;'>
                            <span style='font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#777777;'>
                                Questions about this transaction? Contact us any time at
                                <a href='mailto:support@mightybank.com' style='color:{BrandColor}; text-decoration:none; font-weight:600;'>support@mightybank.com</a>
                            </span>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style='background-color:#fafafa; padding:24px 40px; text-align:center; border-top:1px solid #f0f0f0;'>
                            <div style='font-family: Helvetica, Arial, sans-serif; font-size:12px; color:#9a9a9a; line-height:1.6;'>
                                &copy; {year} {BankName}. All rights reserved.<br/>
                                This is an automated, secure system notification &mdash; please do not reply directly to this email.
                            </div>
                        </td>
                    </tr>

                    <!-- Bottom accent bar -->
                    <tr>
                        <td style='background-color:{BrandColor}; height:6px; line-height:6px; font-size:0;'>&nbsp;</td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------
    private static string DetailRow(string label, string value, bool shaded)
    {
        var bg = shaded ? "#fafafa" : "#ffffff";
        return $@"
            <tr>
                <td style='padding:12px 16px; background-color:{bg}; border-bottom:1px solid #f0f0f0; font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#8a8a8a; font-weight:600; width:42%;'>{label}</td>
                <td style='padding:12px 16px; background-color:{bg}; border-bottom:1px solid #f0f0f0; font-family: Helvetica, Arial, sans-serif; font-size:14px; color:#2d2d2d;'>{value}</td>
            </tr>";
    }

    private static string DetailsTable(string rows)
    {
        return $@"
            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin:24px 0; border:1px solid #f0f0f0; border-radius:8px; overflow:hidden;'>
                {rows}
            </table>";
    }

    private static string AmountBlock(string amount, bool isCredit)
    {
        var color = isCredit ? "#1f8a4c" : BrandColor;
        var bg = isCredit ? "#f0faf3" : "#fdf0f2";
        var sign = isCredit ? "+" : "&minus;";
        return $@"
            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin: 8px 0 28px 0;'>
                <tr>
                    <td align='center' style='background-color:{bg}; border:1px solid {color}22; border-radius:10px; padding:22px;'>
                        <div style='font-family: Helvetica, Arial, sans-serif; font-size:12px; letter-spacing:1.5px; text-transform:uppercase; color:{color}; opacity:0.85; margin-bottom:6px;'>
                            {(isCredit ? "Amount Credited" : "Amount Debited")}
                        </div>
                        <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:32px; font-weight:700; color:{color};'>
                            {sign} &#8358;{amount}
                        </div>
                    </td>
                </tr>
            </table>";
    }

    private static string StatusBadge(string label, string color, string bg)
    {
        return
            $@"<span style='display:inline-block; padding:4px 12px; border-radius:999px; font-size:12px; font-weight:700; letter-spacing:0.3px; color:{color}; background-color:{bg};'>{label}</span>";
    }

    // ------------------------------------------------------------------
    // Welcome email
    // ------------------------------------------------------------------
    public static string GetWelcomeEmailHtml(string customerName, string accountNumber, decimal balance)
    {
        var title = $"Welcome to {BankName}";
        var preheader = $"Your {BankName} account is ready — account number {accountNumber} is now active.";

        var accountCard = $@"
            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin: 8px 0 28px 0;'>
                <tr>
                    <td align='center' style='background-color:{BrandColorDark}; border-radius:10px; padding:28px;'>
                        <div style='font-family: Helvetica, Arial, sans-serif; font-size:11px; letter-spacing:2px; text-transform:uppercase; color:#ffffffaa; margin-bottom:8px;'>
                            Your New Account Number
                        </div>
                        <div style='font-family: ""Courier New"", Courier, monospace; font-size:30px; font-weight:700; color:#ffffff; letter-spacing:4px; margin-bottom:8px;'>
                            {accountNumber}
                        </div>
                        <div style='font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#ffffffcc;'>
                            Opening Balance: <strong style='color:#ffffff;'>&#8358;{balance:N2}</strong>
                        </div>
                    </td>
                </tr>
            </table>";

        var detailsRows = string.Concat(
            DetailRow("Account Holder", customerName, false),
            DetailRow("Account Status", StatusBadge("ACTIVE", "#1f8a4c", "#eafaf0"), true),
            DetailRow("Currency", "Nigerian Naira (&#8358;)", false),
            DetailRow("Date Opened", $"{DateTime.UtcNow:f} (UTC)", true)
        );

        var steps = $@"
            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin-top:10px;'>
                <tr>
                    <td style='padding:14px 0; border-bottom:1px solid #f5f5f5;'>
                        <table role='presentation' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td style='width:32px; vertical-align:top;'>
                                    <div style='width:24px; height:24px; border-radius:50%; background-color:{BrandColor}; color:#ffffff; font-family: Helvetica, Arial, sans-serif; font-size:13px; font-weight:700; text-align:center; line-height:24px;'>1</div>
                                </td>
                                <td style='padding-left:12px;'>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; font-weight:700; color:#2d2d2d;'>Fund Your Account</div>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#777777; margin-top:2px;'>Deposit money using our secure deposit endpoint to get started.</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style='padding:14px 0; border-bottom:1px solid #f5f5f5;'>
                        <table role='presentation' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td style='width:32px; vertical-align:top;'>
                                    <div style='width:24px; height:24px; border-radius:50%; background-color:{BrandColor}; color:#ffffff; font-family: Helvetica, Arial, sans-serif; font-size:13px; font-weight:700; text-align:center; line-height:24px;'>2</div>
                                </td>
                                <td style='padding-left:12px;'>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; font-weight:700; color:#2d2d2d;'>Transfer Instantly</div>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#777777; margin-top:2px;'>Make free, instant transfers to other accounts inside the network.</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style='padding:14px 0;'>
                        <table role='presentation' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td style='width:32px; vertical-align:top;'>
                                    <div style='width:24px; height:24px; border-radius:50%; background-color:{BrandColor}; color:#ffffff; font-family: Helvetica, Arial, sans-serif; font-size:13px; font-weight:700; text-align:center; line-height:24px;'>3</div>
                                </td>
                                <td style='padding-left:12px;'>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; font-weight:700; color:#2d2d2d;'>Track Transactions</div>
                                    <div style='font-family: Helvetica, Arial, sans-serif; font-size:13px; color:#777777; margin-top:2px;'>Retrieve your complete, real-time transaction history at any time.</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>";

        var content = $@"
            <div style='text-align:center; margin-bottom:26px;'>
                <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:22px; font-weight:700; color:#1a1a1a; margin-bottom:8px;'>
                    Welcome aboard, {customerName}
                </div>
                <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; color:#777777;'>
                    We're thrilled to have you with us. Your account is fully active and ready to use.
                </div>
            </div>

            {accountCard}

            <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; font-weight:700; color:#1a1a1a; margin-bottom:4px;'>Account Information</div>
            {DetailsTable(detailsRows)}

            <div style='font-family: Helvetica, Arial, sans-serif; font-size:14px; font-weight:700; color:#1a1a1a; margin: 28px 0 4px 0;'>Quick Onboarding Steps</div>
            {steps}";

        return GetEmailWrapper(preheader, title, content);
    }


    public static string GetAccountUpdateEmailHtml(string customerName, string accountNumber)
    {
        var title = "Account Alert · Profile Updated";
        var preheader = $"Your account {accountNumber} profile details have been updated.";

        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Action", "Profile Update", true),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", false)
        );

        var content = $@"
        <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Profile Details Updated</div>
        <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, the profile details on your account were recently updated. If you did not make this change, please contact support immediately.</p>
        {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }

    public static string GetAccountClosureEmailHtml(string customerName, string accountNumber)
    {
        var title = "Account Alert · Account Closed";
        var preheader = $"Your account {accountNumber} has been closed.";

        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Action", "Account Closure", true),
            DetailRow("Final Balance", "&#8358;0.00", false),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", true)
        );

        var content = $@"
        <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Account Closed</div>
        <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, this confirms that your account has been closed. If you did not request this, please contact support immediately.</p>
        {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }
    
    public static string GetAccountRestoredEmailHtml(string accountNumber, string customerName)
    {
        var title = "Account Alert · Account Restored";
        var preheader = $"Your account {accountNumber} has been restored.";
    
        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Action", "Account Restoration", true),
            DetailRow("Status", "Active", false),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", true)
        );
    
        var content = $@"
        <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Account Restored</div>
        <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, this confirms that your account has been restored and is now active again. If you did not request this, please contact support immediately.</p>
        {DetailsTable(detailsRows)}";
    
        return GetEmailWrapper(preheader, title, content);
    }

    // ------------------------------------------------------------------
    // Deposit alert
    // ------------------------------------------------------------------
    public static string GetDepositAlertEmailHtml(string customerName, string accountNumber, decimal depositAmount,
        decimal newBalance, string reference)
    {
        var title = "Transaction Alert · Deposit Received";
        var preheader = $"Your account was credited with &#8358;{depositAmount:N2}.";

        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Transaction Type", "Deposit (Credit)", true),
            DetailRow("Reference", reference, false),
            DetailRow("Available Balance", $"&#8358;{newBalance:N2}", true),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", false)
        );

        var content = $@"
            <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Transaction Notification</div>
            <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, your account has just been credited with a deposit.</p>
            {AmountBlock($"{depositAmount:N2}", true)}
            {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }

    // ------------------------------------------------------------------
    // Withdrawal alert
    // ------------------------------------------------------------------
    public static string GetWithdrawalAlertEmailHtml(string customerName, string accountNumber,
        decimal withdrawalAmount, decimal newBalance, string reference)
    {
        var title = "Transaction Alert · Withdrawal";
        var preheader = $"A withdrawal of &#8358;{withdrawalAmount:N2} was made on your account.";

        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Transaction Type", "Withdrawal (Debit)", true),
            DetailRow("Reference", reference, false),
            DetailRow("Available Balance", $"&#8358;{newBalance:N2}", true),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", false)
        );

        var content = $@"
            <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Transaction Notification</div>
            <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, a withdrawal has just occurred on your account.</p>
            {AmountBlock($"{withdrawalAmount:N2}", false)}
            {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }

    // ------------------------------------------------------------------
    // Transfer — debit side
    // ------------------------------------------------------------------
    public static string GetTransferDebitAlertEmailHtml(string customerName, string senderAccountNumber,
        string receiverAccountNumber, decimal transferAmount, decimal newBalance, string reference)
    {
        var title = "Transaction Alert · Transfer Sent";
        var preheader = $"You sent &#8358;{transferAmount:N2} to account {receiverAccountNumber}.";

        var detailsRows = string.Concat(
            DetailRow("Sender Account", senderAccountNumber, false),
            DetailRow("Receiver Account", receiverAccountNumber, true),
            DetailRow("Transaction Type", "Transfer (Debit)", false),
            DetailRow("Reference", reference, true),
            DetailRow("Available Balance", $"&#8358;{newBalance:N2}", false),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", true)
        );

        var content = $@"
            <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Fund Transfer Sent</div>
            <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, you've successfully transferred funds to another account.</p>
            {AmountBlock($"{transferAmount:N2}", false)}
            {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }

    // ------------------------------------------------------------------
    // Transfer — credit side
    // ------------------------------------------------------------------
    public static string GetTransferCreditAlertEmailHtml(string customerName, string senderAccountNumber,
        string receiverAccountNumber, decimal transferAmount, decimal newBalance, string reference)
    {
        var title = "Transaction Alert · Transfer Received";
        var preheader = $"You received &#8358;{transferAmount:N2} from account {senderAccountNumber}.";

        var detailsRows = string.Concat(
            DetailRow("Sender Account", senderAccountNumber, false),
            DetailRow("Receiver Account", receiverAccountNumber, true),
            DetailRow("Transaction Type", "Transfer (Credit)", false),
            DetailRow("Reference", reference, true),
            DetailRow("Available Balance", $"&#8358;{newBalance:N2}", false),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", true)
        );

        var content = $@"
            <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Fund Transfer Received</div>
            <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, your account has been credited via an inward fund transfer.</p>
            {AmountBlock($"{transferAmount:N2}", true)}
            {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }


    public static string GetCheckBalanceEmailHtml(string customerName, string accountNumber, decimal balance)
    {
        var title = "Account Alert · Balance Enquiry";
        var preheader = $"Your available balance is &#8358;{balance:N2}.";

        var detailsRows = string.Concat(
            DetailRow("Account Number", accountNumber, false),
            DetailRow("Action", "Balance Enquiry", true),
            DetailRow("Timestamp", $"{DateTime.UtcNow:f} (UTC)", false)
        );

        var content = $@"
        <div style='font-family: Georgia, ""Times New Roman"", serif; font-size:20px; font-weight:700; color:#1a1a1a; margin-bottom:10px;'>Balance Enquiry</div>
        <p style='margin:0 0 4px 0; color:#555555;'>Hello {customerName}, here is a summary of your account balance.</p>
        {AmountBlock($"{balance:N2}", true)}
        {DetailsTable(detailsRows)}";

        return GetEmailWrapper(preheader, title, content);
    }
}