using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
  
namespace OrderV
{
    public class STRUCT_0374
    {
        public STRUCT_0374()
        {

        }
        public int seq { get; set; }
        //user field
        public string formatDateTime { get; set; }
        public string formatLocalTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime localTime { get; set; }
        public string topic { get; set; }
        //raw data 
        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUENCE_NUMBER { get; set; }
        public string TRANSACTION_CODE { get; set;}
        public string ME_GRP_NO { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string TRADING_NUMBER { get; set; }
        public string TRADING_PRICE { get; set; }
        public string TRADING_VOLUMN { get; set; }
        public string SESS_ID { get; set; }
        public string TRADING_DATE { get; set; }
        public string TRADING_TIME { get; set; }
        public string THE_NEARBY_MONTH_TRADING_PRICE { get; set; }
        public string THE_FUTURE_MONTH_TRADING_PRICE { get; set; }
        public string ASK_BID_TYPE_CODE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string MARKET_MAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string POSITION { get; set; }
        public string netPROFIT { get; set; }
        public string netfillCashByPointValue_commision { get; set; }
        public int orderQty { get; set; }
        public STRUCT_0374(string topic, string content)
        {
            try
            {
                lock (MainForm.lockObj)
                {
                    this.seq = MainForm.seq0374++;
                }
                this.topic = topic;
                int offset = 0;
                this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
                this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
                this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
                this.MESSAGE_SEQUENCE_NUMBER = content.Substring(offset, 11); offset += 11;
                this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
                this.ME_GRP_NO = content.Substring(offset, 2); offset += 2;
                this.BOARD_ID = content.Substring(offset, 2); offset += 2;
                this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
                this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
                this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
                this.TRADING_NUMBER = content.Substring(offset, 11); offset += 11;
                this.TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.TRADING_VOLUMN = content.Substring(offset, 10); offset += 10;
                this.SESS_ID = content.Substring(offset, 2); offset += 2;
                this.TRADING_DATE = content.Substring(offset, 8); offset += 8;
                int year = 0;
                int month = 0;
                int day = 0;
                int hours = 0;
                int minute = 0;
                int sec = 0;
                int milli = 0;
                if (!TRADING_DATE.Trim().Equals(""))
                {
                    year = Int32.Parse(TRADING_DATE.Substring(0, 4));
                    month = Int32.Parse(TRADING_DATE.Substring(4, 2));
                    day = Int32.Parse(TRADING_DATE.Substring(6, 2));
                }
                this.TRADING_TIME = content.Substring(offset, 9); offset += 9;
                if (!TRADING_TIME.Trim().Equals(""))
                {
                    hours = Int32.Parse(TRADING_TIME.Substring(0, 2));
                    minute = Int32.Parse(TRADING_TIME.Substring(2, 2));
                    sec = Int32.Parse(TRADING_TIME.Substring(4, 2));
                    milli = Int32.Parse(TRADING_TIME.Substring(6, 3));
                }
                if (!(TRADING_DATE.Trim().Equals("") || TRADING_TIME.Trim().Equals("")))
                {
                    DateTime dateTime = new DateTime(year, month, day, hours, minute, sec, milli);
                }
                this.dateTime = dateTime;
                this.formatDateTime = dateTime.ToString("HH:mm:ss.fff");

                this.localTime = HRDateTime.Now;
                this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");

                this.THE_NEARBY_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.THE_FUTURE_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.ASK_BID_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
                this.MARKET_MAKER_ORDER_TYPE_NUMBER = content.Substring(offset, 11); offset += 11;

                /*
                this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
                this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = content?.Substring(offset, 12); offset += 12;
                this.MEMBER_USE_AREA = content?.Substring(offset, 60); offset += 60;
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class STRUCT_0374_REPLY
    {
        public STRUCT_0374_REPLY()
        {

        }
        public int seq { get; set; }
        public int orderSeq { get; set; }
        public int ticketSeq { get; set; }
        //user field
        public string formatDateTime { get; set; }
        public string formatLocalTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime localTime { get; set; }
        public string topic { get; set; }
        //raw data 
        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUENCE_NUMBER { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string ME_GRP_NO { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string TRADING_NUMBER { get; set; }
        public string TRADING_PRICE { get; set; }
        public string TRADING_VOLUMN { get; set; }
        public string SESS_ID { get; set; }
        public string TRADING_DATE { get; set; }
        public string TRADING_TIME { get; set; }
        public string THE_NEARBY_MONTH_TRADING_PRICE { get; set; }
        public string THE_FUTURE_MONTH_TRADING_PRICE { get; set; }
        public string ASK_BID_TYPE_CODE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string MARKET_MAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string POSITION { get; set; }
        public string netPROFIT { get; set; }
        public string netfillCashByPointValue_commision { get; set; }

        public STRUCT_0374_REPLY(string topic, string content)
        {
            try
            {
                lock (MainForm.lockObj)
                {
                    this.seq = MainForm.seq0374_reply++;
                }
                this.topic = topic;
                int offset = 0;
                this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
                this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
                this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
                this.MESSAGE_SEQUENCE_NUMBER = content.Substring(offset, 11); offset += 11;
                this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
                this.ME_GRP_NO = content.Substring(offset, 2); offset += 2;
                this.BOARD_ID = content.Substring(offset, 2); offset += 2;
                this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
                this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
                this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
                this.TRADING_NUMBER = content.Substring(offset, 11); offset += 11;
                this.TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.TRADING_VOLUMN = content.Substring(offset, 10); offset += 10;
                this.SESS_ID = content.Substring(offset, 2); offset += 2;
                this.TRADING_DATE = content.Substring(offset, 8); offset += 8;
                int year = 0;
                int month = 0;
                int day = 0;
                int hours = 0;
                int minute = 0;
                int sec = 0;
                int milli = 0;
                if (!TRADING_DATE.Trim().Equals(""))
                {
                    year = Int32.Parse(TRADING_DATE.Substring(0, 4));
                    month = Int32.Parse(TRADING_DATE.Substring(4, 2));
                    day = Int32.Parse(TRADING_DATE.Substring(6, 2));
                }
                this.TRADING_TIME = content.Substring(offset, 9); offset += 9;
                if (!TRADING_TIME.Trim().Equals(""))
                {
                    hours = Int32.Parse(TRADING_TIME.Substring(0, 2));
                    minute = Int32.Parse(TRADING_TIME.Substring(2, 2));
                    sec = Int32.Parse(TRADING_TIME.Substring(4, 2));
                    milli = Int32.Parse(TRADING_TIME.Substring(6, 3));
                }
                if (!(TRADING_DATE.Trim().Equals("") || TRADING_TIME.Trim().Equals("")))
                {
                    DateTime dateTime = new DateTime(year, month, day, hours, minute, sec, milli);
                }
                this.dateTime = dateTime;
                this.formatDateTime = dateTime.ToString("HH:mm:ss.fff");

                this.localTime = HRDateTime.Now;
                this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");

                this.THE_NEARBY_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.THE_FUTURE_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
                this.ASK_BID_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
                this.MARKET_MAKER_ORDER_TYPE_NUMBER = content.Substring(offset, 11); offset += 11;

                /*
                this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
                this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = content?.Substring(offset, 12); offset += 12;
                this.MEMBER_USE_AREA = content?.Substring(offset, 60); offset += 60;
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
    [StructLayout(LayoutKind.Auto, Pack = 4)]
    public class STRUCT_0393
    {
        public STRUCT_0393()
        {

        }
        public int seq { get; set; }
        public int ticketSeq { get; set; }
        //user field
        public string formatDateTime { get; set; }
        public string formatLocalTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime localTime { get; set; }
        public string topic { get; set; }

        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY  { get; set; }
        public string MESSAGE_SEQUECE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        /**0000000000*/
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string ASK_BIDTYPE_CODE { get; set; }
        public string MODIFY_OR_CANCEL_TYPE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string ORDER_QUANTITY { get; set; }
        public string ORDER_PRICE { get; set; }
        public string ORDER_TYPE_CODE { get; set; }
        public string ORDER_CONDITION_CODE { get; set; }
        public string MARKETMAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TREASURYSTOCK_STATEMENT_IDENTIFICATION { get; set; }
        public string TREASURYSTOCK_TRADING_METHOD_CODE { get; set; }
        public string ASK_TYPE_CODE { get; set; }
        public string CREDIT_TYPE_CODE { get; set; }
        public string TRUST_PRINCIPAL_TYPE_CODE { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string PROGRAM_TRADING_TYPE_CODE { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string ACCOUNT_TYPE_CODE { get; set; }
        public string ACCOUNT_MARGIN_TYPE_CODE { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string INVESTOR_TYPE_CODE { get; set; }
        public string FOREIGN_INVESTOR_TYPE_CODE { get; set; }
        public string ORD_MEDIA_TP_CODE { get; set; }
        public string ORDER_IDENTIFICATION_INFORMATION { get; set; }
        public string MAC_ADDR { get; set; }
        public string ORDER_DATE { get; set; }
        public string MEMBER_FIRM_ORDER_TIME { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string PROGRAM_ORDER_DECLARE_TYPE_CODE { get; set; }
        public string FILLED_QUANTITY { get; set; }
        public string CANCELE_QUANTITY { get; set; }
        public string CANCELED_QUANTITY { get; set; }
        public string PENDDING_STATE { get; set; }
        public char [] value1;
        public STRUCT_0393(string topic,int ticketNumber,string issue_code,string orderNumber , string org_orderNumber, string side , string orderType , string orderQty , string orderPrice)
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393++;
                MainForm.orderNumber++;
            }
            this.ticketSeq = ticketNumber;
            this.topic = topic;
            this.MSG_LENGTH = "0393";
            this.MSG_HEADER = "XC1120TRDATA000000010000000000000081309098    ";
            this.MSG_DUMMY = "                                                                                  ";
            this.MESSAGE_SEQUECE = "00000000000";
            this.TRANSACTION_CODE = "TCHODR10001";
            this.BOARD_ID = "G1";
            this.MEMBER_NUMBER = "00088";
            this.BRANCH_NUMBER = "00101";
            //this.ORDER_IDENTIFICATION = "0007900001";
            this.ORDER_IDENTIFICATION = orderNumber;
            //this.ORIGINAL_ORDER_IDENTIFICATION = "          ";
            this.ORIGINAL_ORDER_IDENTIFICATION = org_orderNumber;

            //this.ISSUE_CODE = "KR4101LC0006";
            this.ISSUE_CODE = issue_code; ;

            //this.ASK_BIDTYPE_CODE = "2";
            this.ASK_BIDTYPE_CODE = side;

            //this.MODIFY_OR_CANCEL_TYPE = "2";
            this.MODIFY_OR_CANCEL_TYPE = orderType;

            this.ACCOUNT_NUMBER = "000101302346";
            //this.ORDER_QUANTITY = "0000000000";
            this.ORDER_QUANTITY = orderQty;
            //this.ORDER_PRICE = "00000000.00";
            this.ORDER_PRICE = orderPrice;
            this.ORDER_TYPE_CODE = "2";
            this.ORDER_CONDITION_CODE = "0";
            this.MARKETMAKER_ORDER_TYPE_NUMBER = "00000000000";
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = "     ";
            this.TREASURYSTOCK_TRADING_METHOD_CODE = " ";
            this.ASK_TYPE_CODE = "  ";
            this.CREDIT_TYPE_CODE = "  ";
            this.TRUST_PRINCIPAL_TYPE_CODE = "11";
            this.TRUST_COMPANY_NUMBER = "     ";
            this.PROGRAM_TRADING_TYPE_CODE = "00";
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = "            ";
            this.ACCOUNT_TYPE_CODE = "  ";
            this.ACCOUNT_MARGIN_TYPE_CODE = "  ";
            this.COUNTRY_CODE = "   ";
            this.INVESTOR_TYPE_CODE = "    ";
            this.FOREIGN_INVESTOR_TYPE_CODE = "  ";
            this.ORD_MEDIA_TP_CODE = "9";
            this.ORDER_IDENTIFICATION_INFORMATION = "193100140013";
            this.MAC_ADDR = "000F532B7CF1";
            this.ORDER_DATE = "20161201";
            this.MEMBER_FIRM_ORDER_TIME = "081309098";
            this.MEMBER_USE_AREA = "                                                           1";
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = " ";
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
            this.FILLED_QUANTITY = "0";
            this.CANCELE_QUANTITY = "0";
            this.CANCELED_QUANTITY = "0";
            this.PENDDING_STATE = "0";
        }

        public STRUCT_0393(string topic, string issue_code, string orderNumber = "0000000000", string org_orderNumber = "          ", string side = "0", string orderType = "0", string orderQty = "0000000000", string orderPrice = "00000000.00")
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393++;
                MainForm.orderNumber++;
            }
            this.topic = topic;
            this.MSG_LENGTH = "0393";
            this.MSG_HEADER = "XC1120TRDATA000000010000000000000081309098    ";
            this.MSG_DUMMY = "                                                                                  ";
            this.MESSAGE_SEQUECE = "00000000000";
            this.TRANSACTION_CODE = "TCHODR10001";
            this.BOARD_ID = "G1";
            this.MEMBER_NUMBER = "00088";
            this.BRANCH_NUMBER = "00101";
            //this.ORDER_IDENTIFICATION = "0007900001";
            this.ORDER_IDENTIFICATION = orderNumber;
            //this.ORIGINAL_ORDER_IDENTIFICATION = "          ";
            this.ORIGINAL_ORDER_IDENTIFICATION = org_orderNumber;

            //this.ISSUE_CODE = "KR4101LC0006";
            this.ISSUE_CODE = issue_code; ;

            //this.ASK_BIDTYPE_CODE = "2";
            this.ASK_BIDTYPE_CODE = side;

            //this.MODIFY_OR_CANCEL_TYPE = "2";
            this.MODIFY_OR_CANCEL_TYPE = orderType;

            this.ACCOUNT_NUMBER = "000101302346";
            //this.ORDER_QUANTITY = "0000000000";
            this.ORDER_QUANTITY = orderQty;
            //this.ORDER_PRICE = "00000000.00";
            this.ORDER_PRICE = orderPrice;
            this.ORDER_TYPE_CODE = "2";
            this.ORDER_CONDITION_CODE = "0";
            this.MARKETMAKER_ORDER_TYPE_NUMBER = "00000000000";
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = "     ";
            this.TREASURYSTOCK_TRADING_METHOD_CODE = " ";
            this.ASK_TYPE_CODE = "  ";
            this.CREDIT_TYPE_CODE = "  ";
            this.TRUST_PRINCIPAL_TYPE_CODE = "11";
            this.TRUST_COMPANY_NUMBER = "     ";
            this.PROGRAM_TRADING_TYPE_CODE = "00";
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = "            ";
            this.ACCOUNT_TYPE_CODE = "  ";
            this.ACCOUNT_MARGIN_TYPE_CODE = "  ";
            this.COUNTRY_CODE = "   ";
            this.INVESTOR_TYPE_CODE = "    ";
            this.FOREIGN_INVESTOR_TYPE_CODE = "  ";
            this.ORD_MEDIA_TP_CODE = "9";
            this.ORDER_IDENTIFICATION_INFORMATION = "193100140013";
            this.MAC_ADDR = "000F532B7CF1";
            this.ORDER_DATE = "20161201";
            this.MEMBER_FIRM_ORDER_TIME = "081309098";
            this.MEMBER_USE_AREA = "                                                           1";
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = " ";
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
            this.CANCELE_QUANTITY = "0";
            this.CANCELED_QUANTITY = "0";
            this.PENDDING_STATE = "0";
        }

        public STRUCT_0393(string topic, string content,string type)
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393++;
                MainForm.orderNumber++;
            }
            this.topic = topic;
            int offset = 0;
            this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
            this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
            this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
            this.MESSAGE_SEQUECE = content.Substring(offset, 11); offset += 11;
            this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
            this.BOARD_ID = content.Substring(offset, 2); offset += 2;
            this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
            this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
            this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
            this.ASK_BIDTYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.MODIFY_OR_CANCEL_TYPE = content.Substring(offset, 1); offset += 1;
            this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.ORDER_QUANTITY = content.Substring(offset, 10); offset += 10;
            this.ORDER_PRICE = content.Substring(offset, 11); offset += 11;
            this.ORDER_TYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.ORDER_CONDITION_CODE = content.Substring(offset, 1); offset += 1;
            this.MARKETMAKER_ORDER_TYPE_NUMBER = content.Substring(offset, 11); offset += 11;
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = content.Substring(offset, 5); offset += 5;
            this.TREASURYSTOCK_TRADING_METHOD_CODE = content.Substring(offset, 1); offset += 1;
            this.ASK_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.CREDIT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.TRUST_PRINCIPAL_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
            this.PROGRAM_TRADING_TYPE_CODE = content.Substring(offset, 2); offset += 2 ;
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.ACCOUNT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.ACCOUNT_MARGIN_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.COUNTRY_CODE = content.Substring(offset, 3); offset += 3;
            this.INVESTOR_TYPE_CODE = content.Substring(offset, 4); offset += 4;
            this.FOREIGN_INVESTOR_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.ORD_MEDIA_TP_CODE = content.Substring(offset, 1); offset += 1;
            this.ORDER_IDENTIFICATION_INFORMATION = content.Substring(offset, 12); offset += 12;
            this.MAC_ADDR = content.Substring(offset, 12); offset += 12;
            this.ORDER_DATE = content.Substring(offset, 8); offset += 8;
            this.MEMBER_FIRM_ORDER_TIME = content.Substring(offset, 9); offset += 9;
            this.MEMBER_USE_AREA = content.Substring(offset, 60); offset += 60;
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
            this.PENDDING_STATE = "0";

        }

        public STRUCT_0393(string topic , string price , string qty , string type)
        {

        }
        
        public string toKRXFORMAT()
        {
            return MSG_LENGTH + MSG_HEADER+ MSG_DUMMY+ MESSAGE_SEQUECE+ TRANSACTION_CODE+ BOARD_ID+ MEMBER_NUMBER+ BRANCH_NUMBER+ ORDER_IDENTIFICATION+ ORIGINAL_ORDER_IDENTIFICATION+ ISSUE_CODE+ 
                   ASK_BIDTYPE_CODE+ MODIFY_OR_CANCEL_TYPE+ ACCOUNT_NUMBER+ ORDER_QUANTITY+ ORDER_PRICE+ ORDER_TYPE_CODE + ORDER_CONDITION_CODE+ MARKETMAKER_ORDER_TYPE_NUMBER+ TREASURYSTOCK_STATEMENT_IDENTIFICATION+
                   TREASURYSTOCK_TRADING_METHOD_CODE+ ASK_TYPE_CODE+ CREDIT_TYPE_CODE+ TRUST_PRINCIPAL_TYPE_CODE+ TRUST_COMPANY_NUMBER+ PROGRAM_TRADING_TYPE_CODE+ SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER+
                   ACCOUNT_TYPE_CODE+ ACCOUNT_MARGIN_TYPE_CODE+ COUNTRY_CODE+ INVESTOR_TYPE_CODE+ FOREIGN_INVESTOR_TYPE_CODE+ ORD_MEDIA_TP_CODE+ ORDER_IDENTIFICATION_INFORMATION+ MAC_ADDR+ ORDER_DATE+
                   MEMBER_FIRM_ORDER_TIME+ MEMBER_USE_AREA+ PROGRAM_ORDER_DECLARE_TYPE_CODE;
        }

    }
    public class STRUCT_0393_CANCEL
    {
        public STRUCT_0393_CANCEL()
        {

        }
        public int seq { get; set; }
        public int ticketSeq { get; set; }
        //user field
        public string formatDateTime { get; set; }
        public string formatLocalTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime localTime { get; set; }
        public string topic { get; set; }

        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUECE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        /**0000000000*/
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string ASK_BIDTYPE_CODE { get; set; }
        public string MODIFY_OR_CANCEL_TYPE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string ORDER_QUANTITY { get; set; }
        public string ORDER_PRICE { get; set; }
        public string ORDER_TYPE_CODE { get; set; }
        public string ORDER_CONDITION_CODE { get; set; }
        public string MARKETMAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TREASURYSTOCK_STATEMENT_IDENTIFICATION { get; set; }
        public string TREASURYSTOCK_TRADING_METHOD_CODE { get; set; }
        public string ASK_TYPE_CODE { get; set; }
        public string CREDIT_TYPE_CODE { get; set; }
        public string TRUST_PRINCIPAL_TYPE_CODE { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string PROGRAM_TRADING_TYPE_CODE { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string ACCOUNT_TYPE_CODE { get; set; }
        public string ACCOUNT_MARGIN_TYPE_CODE { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string INVESTOR_TYPE_CODE { get; set; }
        public string FOREIGN_INVESTOR_TYPE_CODE { get; set; }
        public string ORD_MEDIA_TP_CODE { get; set; }
        public string ORDER_IDENTIFICATION_INFORMATION { get; set; }
        public string MAC_ADDR { get; set; }
        public string ORDER_DATE { get; set; }
        public string MEMBER_FIRM_ORDER_TIME { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string PROGRAM_ORDER_DECLARE_TYPE_CODE { get; set; }
        public string FILLED_QUANTITY { get; set; }
        public string CANCELED_QUANTITY { get; set; }
        public string REJECT_CANCEL_QUANTITY { get; set; }
        public STRUCT_0393_CANCEL(string topic, int ticketNumber, string issue_code, string orderNumber = "0000000000", string org_orderNumber = "          ", string side = "0", string orderType = "0", string orderQty = "0000000000", string orderPrice = "00000000.00")
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393_cancel++;
                MainForm.orderNumber++;
            }
            this.ticketSeq = ticketNumber;
            this.topic = topic;
            this.MSG_LENGTH = "0393";
            this.MSG_HEADER = "XC1120TRDATA000000010000000000000081309098    ";
            this.MSG_DUMMY = "                                                                                  ";
            this.MESSAGE_SEQUECE = "00000000000";
            this.TRANSACTION_CODE = "TCHODR10001";
            this.BOARD_ID = "G1";
            this.MEMBER_NUMBER = "00088";
            this.BRANCH_NUMBER = "00101";
            //this.ORDER_IDENTIFICATION = "0007900001";
            this.ORDER_IDENTIFICATION = orderNumber;
            //this.ORIGINAL_ORDER_IDENTIFICATION = "          ";
            this.ORIGINAL_ORDER_IDENTIFICATION = org_orderNumber;

            //this.ISSUE_CODE = "KR4101LC0006";
            this.ISSUE_CODE = issue_code; ;

            //this.ASK_BIDTYPE_CODE = "2";
            this.ASK_BIDTYPE_CODE = side;

            //this.MODIFY_OR_CANCEL_TYPE = "2";
            this.MODIFY_OR_CANCEL_TYPE = orderType;

            this.ACCOUNT_NUMBER = "000101302346";
            //this.ORDER_QUANTITY = "0000000000";
            this.ORDER_QUANTITY = orderQty;
            //this.ORDER_PRICE = "00000000.00";
            this.ORDER_PRICE = orderPrice;
            this.ORDER_TYPE_CODE = "2";
            this.ORDER_CONDITION_CODE = "0";
            this.MARKETMAKER_ORDER_TYPE_NUMBER = "00000000000";
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = "     ";
            this.TREASURYSTOCK_TRADING_METHOD_CODE = " ";
            this.ASK_TYPE_CODE = "  ";
            this.CREDIT_TYPE_CODE = "  ";
            this.TRUST_PRINCIPAL_TYPE_CODE = "11";
            this.TRUST_COMPANY_NUMBER = "     ";
            this.PROGRAM_TRADING_TYPE_CODE = "00";
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = "            ";
            this.ACCOUNT_TYPE_CODE = "  ";
            this.ACCOUNT_MARGIN_TYPE_CODE = "  ";
            this.COUNTRY_CODE = "   ";
            this.INVESTOR_TYPE_CODE = "    ";
            this.FOREIGN_INVESTOR_TYPE_CODE = "  ";
            this.ORD_MEDIA_TP_CODE = "9";
            this.ORDER_IDENTIFICATION_INFORMATION = "193100140013";
            this.MAC_ADDR = "000F532B7CF1";
            this.ORDER_DATE = "20161201";
            this.MEMBER_FIRM_ORDER_TIME = "081309098";
            this.MEMBER_USE_AREA = "                                                           1";
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = " ";
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
            this.FILLED_QUANTITY = "0";
            this.CANCELED_QUANTITY = "0";
            this.REJECT_CANCEL_QUANTITY = "0";
        }

        public STRUCT_0393_CANCEL(string topic, string issue_code, string orderNumber = "0000000000", string org_orderNumber = "          ", string side = "0", string orderType = "0", string orderQty = "0000000000", string orderPrice = "00000000.00")
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393++;
                MainForm.orderNumber++;
            }
            this.topic = topic;
            this.MSG_LENGTH = "0393";
            this.MSG_HEADER = "XC1120TRDATA000000010000000000000081309098    ";
            this.MSG_DUMMY = "                                                                                  ";
            this.MESSAGE_SEQUECE = "00000000000";
            this.TRANSACTION_CODE = "TCHODR10001";
            this.BOARD_ID = "G1";
            this.MEMBER_NUMBER = "00088";
            this.BRANCH_NUMBER = "00101";
            //this.ORDER_IDENTIFICATION = "0007900001";
            this.ORDER_IDENTIFICATION = orderNumber;
            //this.ORIGINAL_ORDER_IDENTIFICATION = "          ";
            this.ORIGINAL_ORDER_IDENTIFICATION = org_orderNumber;

            //this.ISSUE_CODE = "KR4101LC0006";
            this.ISSUE_CODE = issue_code; ;

            //this.ASK_BIDTYPE_CODE = "2";
            this.ASK_BIDTYPE_CODE = side;

            //this.MODIFY_OR_CANCEL_TYPE = "2";
            this.MODIFY_OR_CANCEL_TYPE = orderType;

            this.ACCOUNT_NUMBER = "000101302346";
            //this.ORDER_QUANTITY = "0000000000";
            this.ORDER_QUANTITY = orderQty;
            //this.ORDER_PRICE = "00000000.00";
            this.ORDER_PRICE = orderPrice;
            this.ORDER_TYPE_CODE = "2";
            this.ORDER_CONDITION_CODE = "0";
            this.MARKETMAKER_ORDER_TYPE_NUMBER = "00000000000";
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = "     ";
            this.TREASURYSTOCK_TRADING_METHOD_CODE = " ";
            this.ASK_TYPE_CODE = "  ";
            this.CREDIT_TYPE_CODE = "  ";
            this.TRUST_PRINCIPAL_TYPE_CODE = "11";
            this.TRUST_COMPANY_NUMBER = "     ";
            this.PROGRAM_TRADING_TYPE_CODE = "00";
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = "            ";
            this.ACCOUNT_TYPE_CODE = "  ";
            this.ACCOUNT_MARGIN_TYPE_CODE = "  ";
            this.COUNTRY_CODE = "   ";
            this.INVESTOR_TYPE_CODE = "    ";
            this.FOREIGN_INVESTOR_TYPE_CODE = "  ";
            this.ORD_MEDIA_TP_CODE = "9";
            this.ORDER_IDENTIFICATION_INFORMATION = "193100140013";
            this.MAC_ADDR = "000F532B7CF1";
            this.ORDER_DATE = "20161201";
            this.MEMBER_FIRM_ORDER_TIME = "081309098";
            this.MEMBER_USE_AREA = "                                                           1";
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = " ";
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
        }

        public STRUCT_0393_CANCEL(string topic, string content, string type)
        {
            lock (MainForm.lockObj)
            {
                this.seq = MainForm.seq0393++;
                MainForm.orderNumber++;
            }
            this.topic = topic;
            int offset = 0;
            this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
            this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
            this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
            this.MESSAGE_SEQUECE = content.Substring(offset, 11); offset += 11;
            this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
            this.BOARD_ID = content.Substring(offset, 2); offset += 2;
            this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
            this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
            this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
            this.ASK_BIDTYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.MODIFY_OR_CANCEL_TYPE = content.Substring(offset, 1); offset += 1;
            this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.ORDER_QUANTITY = content.Substring(offset, 10); offset += 10;
            this.ORDER_PRICE = content.Substring(offset, 11); offset += 11;
            this.ORDER_TYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.ORDER_CONDITION_CODE = content.Substring(offset, 1); offset += 1;
            this.MARKETMAKER_ORDER_TYPE_NUMBER = content.Substring(offset, 11); offset += 11;
            this.TREASURYSTOCK_STATEMENT_IDENTIFICATION = content.Substring(offset, 5); offset += 5;
            this.TREASURYSTOCK_TRADING_METHOD_CODE = content.Substring(offset, 1); offset += 1;
            this.ASK_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.CREDIT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.TRUST_PRINCIPAL_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
            this.PROGRAM_TRADING_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.ACCOUNT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.ACCOUNT_MARGIN_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.COUNTRY_CODE = content.Substring(offset, 3); offset += 3;
            this.INVESTOR_TYPE_CODE = content.Substring(offset, 4); offset += 4;
            this.FOREIGN_INVESTOR_TYPE_CODE = content.Substring(offset, 2); offset += 2;
            this.ORD_MEDIA_TP_CODE = content.Substring(offset, 1); offset += 1;
            this.ORDER_IDENTIFICATION_INFORMATION = content.Substring(offset, 12); offset += 12;
            this.MAC_ADDR = content.Substring(offset, 12); offset += 12;
            this.ORDER_DATE = content.Substring(offset, 8); offset += 8;
            this.MEMBER_FIRM_ORDER_TIME = content.Substring(offset, 9); offset += 9;
            this.MEMBER_USE_AREA = content.Substring(offset, 60); offset += 60;
            this.PROGRAM_ORDER_DECLARE_TYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.localTime = HRDateTime.Now;
            this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
        }

        public STRUCT_0393_CANCEL(string topic, string price, string qty, string type)
        {

        }

        public string toKRXFORMAT()
        {
            return MSG_LENGTH + MSG_HEADER + MSG_DUMMY + MESSAGE_SEQUECE + TRANSACTION_CODE + BOARD_ID + MEMBER_NUMBER + BRANCH_NUMBER + ORDER_IDENTIFICATION + ORIGINAL_ORDER_IDENTIFICATION + ISSUE_CODE +
                   ASK_BIDTYPE_CODE + MODIFY_OR_CANCEL_TYPE + ACCOUNT_NUMBER + ORDER_QUANTITY + ORDER_PRICE + ORDER_TYPE_CODE + ORDER_CONDITION_CODE + MARKETMAKER_ORDER_TYPE_NUMBER + TREASURYSTOCK_STATEMENT_IDENTIFICATION +
                   TREASURYSTOCK_TRADING_METHOD_CODE + ASK_TYPE_CODE + CREDIT_TYPE_CODE + TRUST_PRINCIPAL_TYPE_CODE + TRUST_COMPANY_NUMBER + PROGRAM_TRADING_TYPE_CODE + SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER +
                   ACCOUNT_TYPE_CODE + ACCOUNT_MARGIN_TYPE_CODE + COUNTRY_CODE + INVESTOR_TYPE_CODE + FOREIGN_INVESTOR_TYPE_CODE + ORD_MEDIA_TP_CODE + ORDER_IDENTIFICATION_INFORMATION + MAC_ADDR + ORDER_DATE +
                   MEMBER_FIRM_ORDER_TIME + MEMBER_USE_AREA + PROGRAM_ORDER_DECLARE_TYPE_CODE;
        }

    }

    public class STRUCT_0419
    {
        public STRUCT_0419()
        {

        }
        public int seq { get; set; }
        public int ticketSeq { get; set; }
        //user field
        public string formatDateTime { get; set; }
        public string formatLocalTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime localTime { get; set; }
        public string topic { get; set; }
        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUENCE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string ME_GRP_NO { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string ASK_BID_TYPE_CODE { get; set; }
        public string MODIFY_OR_CANCEL_TYPE_CODE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string ORDER_QUANTITY { get; set; }
        public string ORDER_PRICE { get; set; }
        public string ORDER_TYPE_CODE { get; set; }
        public string ORDER_CONDITION_CODE { get; set; }
        public string MARKETMAKER_ORDER_TYPE { get; set; }
        public string TREASURY_STOCK_STATEMENT_IDENTIFICATION { get; set; }
        public string TREASURY_STOCK_TRADING_METHOD_CODE { get; set; }
        public string ASK_TYPE_CODE { get; set; }
        public string CREDIT_TYPE_CODE { get; set; }
        public string TRUST_PRINCIPAL_TYPE_CODE { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string PROGRAM_TRADING_TYPE_CODE { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_Number { get; set; }
        public string ACCOUNT_TYPE_CODE { get; set; }
        public string ACCOUNT_MARGIN_TYPE_CODE { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string INVESTOR_TYPE_CODE { get; set; }
        public string FOREIGN_INVESTOR_TYPE_CODE { get; set; }
        public string ORD_MEDIA_TP_CD { get; set; }
        public string ORDER_IDENTIFICATION_INFORMATION { get; set; }
        public string MAC_ADDR { get; set; }
        public string ORDER_DATE { get; set; }
        public string MEMBER_FIRM_ORDER_TIME { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string ORD_ACPT_TM { get; set; }
        public string REAL_MODIFY_OR_CANCEL_ORDER_QUANTITY { get; set; }
        public string AUTOMATIC_CANCELLATION_PROCESS_TYPE_CODE { get; set; }
        public string ORDER_REJECTED_REASON_CODE { get; set; }
        public string PROGRAM_ORDER_DECLARE_TYPE_CODE 				{get; set;}

        public STRUCT_0419(string topic, string content)
        {
            try
            {
                lock (MainForm.lockObj)
                {
                    this.seq = MainForm.seq0419++;
                }
                this.topic = topic;
                int offset = 0;
                this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
                this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
                this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
                this.MESSAGE_SEQUENCE = content.Substring(offset, 11); offset += 11;
                this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
                this.ME_GRP_NO = content.Substring(offset, 2); offset += 2;
                this.BOARD_ID = content.Substring(offset, 2); offset += 2;
                this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
                this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
                this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
                this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
                this.ASK_BID_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.MODIFY_OR_CANCEL_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
                this.ORDER_QUANTITY = content.Substring(offset, 10); offset += 10;
                this.ORDER_PRICE = content.Substring(offset, 11); offset += 11;
                this.ORDER_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.ORDER_CONDITION_CODE = content.Substring(offset, 1); offset += 1;
                this.MARKETMAKER_ORDER_TYPE = content.Substring(offset, 11); offset += 11;
                this.TREASURY_STOCK_STATEMENT_IDENTIFICATION = content.Substring(offset, 5); offset += 5;
                this.TREASURY_STOCK_TRADING_METHOD_CODE = content.Substring(offset, 1); offset += 1;
                this.ASK_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.CREDIT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.TRUST_PRINCIPAL_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
                this.PROGRAM_TRADING_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_Number = content.Substring(offset, 12); offset += 12;
                this.ACCOUNT_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.ACCOUNT_MARGIN_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.COUNTRY_CODE = content.Substring(offset, 3); offset += 3;
                this.INVESTOR_TYPE_CODE = content.Substring(offset, 4); offset += 4;
                this.FOREIGN_INVESTOR_TYPE_CODE = content.Substring(offset, 2); offset += 2;
                this.ORD_MEDIA_TP_CD = content.Substring(offset, 1); offset += 1;
                this.ORDER_IDENTIFICATION_INFORMATION = content.Substring(offset, 12); offset += 12;
                this.MAC_ADDR = content.Substring(offset, 12); offset += 12;
                this.ORDER_DATE = content.Substring(offset, 8); offset += 8;
                this.MEMBER_FIRM_ORDER_TIME = content.Substring(offset, 9); offset += 9;
                this.MEMBER_USE_AREA = content.Substring(offset, 60); offset += 60;
                this.ORD_ACPT_TM = content.Substring(offset, 9); offset += 9;
                this.REAL_MODIFY_OR_CANCEL_ORDER_QUANTITY = content.Substring(offset, 10); offset += 10;
                this.AUTOMATIC_CANCELLATION_PROCESS_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.ORDER_REJECTED_REASON_CODE = content.Substring(offset, 4); offset += 4;
                this.PROGRAM_ORDER_DECLARE_TYPE_CODE = content.Substring(offset, 1); offset += 1;
                this.localTime = HRDateTime.Now;
                this.formatLocalTime = localTime.ToString("HH:mm:ss.fff");
                int year = 0;
                int month = 0;
                int day = 0;
                int hours = 0;
                int minute = 0;
                int sec = 0;
                int milli = 0;
                if (!ORDER_DATE.Trim().Equals(""))
                {
                    year = Int32.Parse(ORDER_DATE.Substring(0, 4));
                    month = Int32.Parse(ORDER_DATE.Substring(4, 2));
                    day = Int32.Parse(ORDER_DATE.Substring(6, 2));
                }
                if (!ORD_ACPT_TM.Trim().Equals(""))
                {
                    hours = Int32.Parse(ORD_ACPT_TM.Substring(0, 2));
                    minute = Int32.Parse(ORD_ACPT_TM.Substring(2, 2));
                    sec = Int32.Parse(ORD_ACPT_TM.Substring(4, 2));
                    milli = Int32.Parse(ORD_ACPT_TM.Substring(6, 3));
                }
                if (!(ORDER_DATE.Trim().Equals("") || ORD_ACPT_TM.Trim().Equals("")))
                {
                    DateTime dateTime = new DateTime(year, month, day, hours, minute, sec, milli);
                }
                this.dateTime = dateTime;
                this.formatDateTime = dateTime.ToString("HH:mm:ss.fff");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class 로직
    {
        public int 시퀀스 { get; set; }
        public string 토픽 { get; set; }
        public string 시간 { get; set; }
        public string 가격 { get; set; }
        public int 수량 { get; set; }
        public int 포지션 { get; set; }
        public double 순손익 { get; set; }
        public double 자료 { get; set; } 
    }

    public class 운용
    {
        public int 시퀀스1 { get; set; }
        public int 시퀀스 { get; set; }
        public string 토픽 { get; set; }
        public string 시간 { get; set; }
        public string 가격 { get; set; }
        public int 수량 { get; set; }
        public int 포지션 { get; set; }
        public double 순손익 { get; set; }
        public double 자료 { get; set; }
    }

    public class 클라이언트
    {
        public int 시퀀스 { get; set; }
        public string 토픽 { get; set; }
        public string 시간 { get; set; }
        public string 주문번호 { get; set; }
        public string 종목 { get; set; }
        public string 매매구분 { get; set; }
        public string 가격 { get; set; }
        public int 체결 { get; set; }
        public string 시간1 { get; set; }
    }

    public class 신규주문
    {
        public int 시퀀스1 { get; set; }
        public int 시퀀스 { get; set; }
        public string 토픽 { get; set; }
        public string 시간 { get; set; }
        public string 주문번호 { get; set; }
        public string 원주문번호 { get; set; }
        public string 종목 { get; set; }
        public string 매매구분 { get; set; }
        public string 가격 { get; set; }
        public int 수량 { get; set; }
        public int 체결 { get; set; }
        public int 취소요청 { get; set; }
        public int 취소확인 { get; set; }
    }

    public class 취소주문
    {
        public int 시퀀스1 { get; set; }
        public int 시퀀스 { get; set; }
        public string 토픽 { get; set; }
        public string 시간 { get; set; }
        public string 주문번호 { get; set; }
        public string 원주문번호 { get; set; }
        public string 종목 { get; set; }
        public string 매매구분 { get; set; }
        public string 가격 { get; set; }
        public int 수량 { get; set; }
        public int 취소확인 { get; set; }
        public int 취소거부 { get; set; }
    }
}
