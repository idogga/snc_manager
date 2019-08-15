using System;

namespace snc_bonus_operator
{
    public static class Request
    {
        #region Названия команд для запросов к HTTP службе
        // По общему HTTPS
        public const string MobileRssRequest = "MobileRssRequest";
        public const string MobileIssuerRequest = "MobileIssuerRequest";
        public const string MobileMapRequest = "MobileMapRequest";
        public const string MobilePriceRequest = "MobilePriceRequest";
        public const string MobileAuthorize = "MobileAuthorize";
        public const string MobileSystemRequest = "MobileSystemRequest";

        // По уникальному HTTPS
        public const string MobileOrderRequest = "MobileOrderRequest";
        public const string MobileCardInfoRequest = "MobileCardInfoRequest";
        public const string MobileHistoryRequest = "MobileHistoryRequest";
        public const string MobileNotificationsRequest = "MobileNotificationsRequest";
        public const string MobileInfoRequest = "MobileInfoRequest";
        #endregion

        #region Шаблоны команд (предварительно заполненные запросы)

        private static string requestUserDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>{0}</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{1}</DetailsSelectName>
		<DetailsValue>{2}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";


        public static string rssRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileRssRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string issuerRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileIssuerRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string mapRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileMapRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>Map</DetailsSelectName>
		<DetailsValue></DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string priceRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobilePriceRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>Price</DetailsSelectName>
		<DetailsValue>{0},{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string priceRequestUserDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobilePriceRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>Price</DetailsSelectName>
		<DetailsValue>{0},{1},{2}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";


        private static string orderRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>{5}</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileOrderRequest</SelectName>
		<GraphicalNumber>{0}</GraphicalNumber>
		<CardApplicationKey>0</CardApplicationKey>
        <COD_Q>{3}</COD_Q>
		<COD_AZS>{4}</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>{1}</DetailsKey>
		<DetailsSelectName></DetailsSelectName>
		<DetailsValue>{2}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string cardRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileCardInfoRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";


        public static string mobileRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>AzsInfo</DetailsSelectName>
		<DetailsValue></DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string infoRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>InfoRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<CardInfo>
		<CardInfoKey>1</CardInfoKey>
		<CardKey>1</CardKey>
		<CardStateKey>0</CardStateKey>
		<GraphicalNumber/>
		<Name>CardInfoRequest</Name>
		<COD_A>1</COD_A>
		<COD_O>1</COD_O>
		<Note/>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
		<OrganizationState>0</OrganizationState>
		<OrganizationBalance>0</OrganizationBalance>
		<PCState>0</PCState>
		<SyntheticAccountKey>0</SyntheticAccountKey>
	</CardInfo>
</RequestDS>";
        public static string systemRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileSystemRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";


        public static string authorizeRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileAuthorize</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";


        public static string mobileInfoRequestDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>MobileInfoRequest</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";

        public static string verifyTransactionsDSxml = @"<RequestDS>
	<Request>
		<RequestKey>1</RequestKey>
		<ShopRequestKey>1</ShopRequestKey>
		<CommandCode>1</CommandCode>
		<SelectName>VerifyTransactions</SelectName>
		<COD_Q>1</COD_Q>
		<COD_AZS>1</COD_AZS>
		<COD_A>1</COD_A>
	</Request>
	<Details>
		<DetailsKey>1</DetailsKey>
		<DetailsSelectName>{0}</DetailsSelectName>
		<DetailsValue>{1}</DetailsValue>
		<RequestKey>0</RequestKey>
		<ShopRequestKey>0</ShopRequestKey>
	</Details>
</RequestDS>";
        #endregion

        #region Форматирование и заполнение шаблонов
        public static string GetOrderXml(string graphicalNumber, int detailsKey, string detailValue, string issuerId, string azsId, string requestKey)
        {
            return string.Format(orderRequestDSxml, graphicalNumber, detailsKey, detailValue, issuerId, azsId, requestKey);
        }

        public static string GetPriceXml(int issuerId, int azsId, string userId = "")
        {
            if (userId == "")
                return string.Format(priceRequestDSxml, issuerId, azsId);
            else
                return string.Format(priceRequestUserDSxml, issuerId, azsId, userId);
        }

        public static string GetIssuerXml(string tag, string detailValue)
        {
            return string.Format(issuerRequestDSxml, tag, detailValue);
        }

        internal static string GetRssRequestDSxml(string typeTag, string newsInfo)
        {
            return string.Format(rssRequestDSxml, typeTag, newsInfo);
        }
        internal static string GetCardRequestDSxml(string typeTag, string cardsInfo)
        {
            return string.Format(cardRequestDSxml, typeTag, cardsInfo);
        }
        internal static string GetSystemInfoXml(string typeTag, string detailValue)
        {
            return string.Format(systemRequestDSxml, typeTag, detailValue);
        }
        internal static string GetAuthXml(string typeTag, string detailValue)
        {
            return string.Format(authorizeRequestDSxml, typeTag, detailValue);
        }
        internal static string GetMobileInfoXml(string typeTag, string detailValue)
        {
            return string.Format(mobileInfoRequestDSxml, typeTag, detailValue);
        }
        internal static string GetVerifyTransactionXml(string typeTag, string detailValue)
        {
            return string.Format(verifyTransactionsDSxml, typeTag, detailValue);
        }

        #endregion

        internal static string GetRequestDSxml(RequestTagEnum tag, string data)
        {
            return string.Format(requestUserDSxml, GetCommandName(tag), GetSubCommandName(tag), data);
        }
        private static string GetCommandName(RequestTagEnum tag)
        {
            var str = "";
            switch (tag)
            {
                case RequestTagEnum.CheckAzs:
                case RequestTagEnum.CheckNozzle:
                case RequestTagEnum.CheckAzsSeller:
                    str = "MobilePriceRequest";
                    break;
                case RequestTagEnum.Limitations:
                case RequestTagEnum.BonusCount:
                case RequestTagEnum.CalculateFuelSale:
                case RequestTagEnum.SetLoggs:
                case RequestTagEnum.GetAvailableGoods:
                    str = "MobileSystemRequest";
                    break;
                case RequestTagEnum.Sales:
                case RequestTagEnum.News:
                    str = "MobileRssRequest";
                    break;
                case RequestTagEnum.ReqPhoneEmail:
                case RequestTagEnum.SetDevice:
                case RequestTagEnum.Profile:
                case RequestTagEnum.KeyWord:
                    str = "MobileInfoRequest";
                    break;
                case RequestTagEnum.GetBonusTransaction:
                case RequestTagEnum.GetBonusBill:
                case RequestTagEnum.GetMyBonusPrograms:
                case RequestTagEnum.SetPrograms:
                case RequestTagEnum.SetOldPrograms:
                case RequestTagEnum.BonusFilteredTransactions:
                case RequestTagEnum.BonusApp:
                case RequestTagEnum.Block:
                case RequestTagEnum.ChangeRequisites:
                case RequestTagEnum.AllTransactionsSeller:
                    str = "MobileCardInfoRequest";
                    break;
                case RequestTagEnum.RegNew:
                case RequestTagEnum.Register:
                case RequestTagEnum.GetPrograms:
                case RequestTagEnum.BlockManager:
                case RequestTagEnum.RegManager:
                case RequestTagEnum.GetMyCrew:
                case RequestTagEnum.SellerReloadPassword:
                    str = "MobileAuthorize";
                    break;
                case RequestTagEnum.FilteredShops:
                case RequestTagEnum.GetAzsInfo:
                    str = "MobileMapRequest";
                    break;
                default:
                    throw new Exception("Неизвестный тип");
            }
            return str;
        }

        private static string GetSubCommandName(RequestTagEnum tag)
        {
            var str = "";
            switch (tag)
            {
                case RequestTagEnum.Block:
                    str = "Block";
                    break;
                case RequestTagEnum.BonusApp:
                    str = "BonusApp";
                    break;
                case RequestTagEnum.BonusCount:
                    str = "BonusCount";
                    break;
                case RequestTagEnum.AllTransactionsSeller:
                    str = "AllTransactionsSeller";
                    break;
                case RequestTagEnum.CalculateFuelSale:
                    str = "CalculateFuelSale";
                    break;
                case RequestTagEnum.SetLoggs:
                    str = "SetLoggs";
                    break;
                case RequestTagEnum.ChangeRequisites:
                    str = "ChangeRequisites";
                    break;
                case RequestTagEnum.CheckAzs:
                    str = "CheckAzs";
                    break;
                case RequestTagEnum.CheckNozzle:
                    str = "CheckNozzle";
                    break;
                case RequestTagEnum.GetBonusBill:
                    str = "GetBonusBill";
                    break;
                case RequestTagEnum.GetBonusTransaction:
                    str = "GetBonusTransaction";
                    break;
                case RequestTagEnum.GetMyBonusPrograms:
                    str = "GetMyBonusPrograms";
                    break;
                case RequestTagEnum.GetPrograms:
                    str = "GetPrograms";
                    break;
                case RequestTagEnum.KeyWord:
                    str = "KeyWord";
                    break;
                case RequestTagEnum.Limitations:
                    str = "Limitations";
                    break;
                case RequestTagEnum.News:
                    str = "News";
                    break;
                case RequestTagEnum.Profile:
                    str = "Profile";
                    break;
                case RequestTagEnum.GetAvailableGoods:
                    str = "GetAvailableGoods";
                    break;
                case RequestTagEnum.Register:
                    str = "Register";
                    break;
                case RequestTagEnum.RegNew:
                    str = "RegNew";
                    break;
                case RequestTagEnum.ReqPhoneEmail:
                    str = "ReqPhoneEmail";
                    break;
                case RequestTagEnum.Sales:
                    str = "Sales";
                    break;
                case RequestTagEnum.SetDevice:
                    str = "SetDevice";
                    break;
                case RequestTagEnum.SetOldPrograms:
                    str = "SetOldPrograms";
                    break;
                case RequestTagEnum.SetPrograms:
                    str = "SetPrograms";
                    break;
                case RequestTagEnum.FilteredShops:
                    str = "FilteredShops";
                    break;
                case RequestTagEnum.BlockManager:
                    str = "BlockManager";
                    break;
                case RequestTagEnum.RegManager:
                    str = "RegManager";
                    break;
                case RequestTagEnum.GetMyCrew:
                    str = "GetMyCrew";
                    break;
                case RequestTagEnum.CheckAzsSeller:
                    str = "CheckAzsSeller";
                    break;
                case RequestTagEnum.SellerReloadPassword:
                    str = "SellerReloadPassword";
                    break;
                case RequestTagEnum.GetAzsInfo:
                    str = "GetAzsInfo";
                    break;
            }
            return str;
        }


    }
}
