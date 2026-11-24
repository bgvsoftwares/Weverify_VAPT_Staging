using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class PQDrugTestVer
    {
        public PQDrugTestVer()
        {
            DrugTestVerRouID = 0;
            DrugTestRowID = 0;
            DT_Cand_Name                        =string.Empty;
            DT_Amphetamine                      =string.Empty;
            DT_Barbiturate                      =string.Empty;
            DT_Benzodiazepine                   =string.Empty;
            DT_Cannabinoids                     =string.Empty;
            DT_Cocaine                          =string.Empty;
            DT_Date_Verf                        =string.Empty;
            DT_Marijuana                        =string.Empty;
            DT_Marijuana_Cannabinoids           =string.Empty;
            DT_Methadone                        =string.Empty;
            DT_Methamphetamine                  =string.Empty;
            DT_Methaqualone                     =string.Empty;
            DT_Morphine_Opiate                  =string.Empty;
            DT_Opiates                          =string.Empty;
            DT_Oxycodone                        =string.Empty;
            DT_Phencyclidine                    =string.Empty;
            DT_Propoxyphene                     =string.Empty;
            DT_Tricyclic_Antidepressant         =string.Empty;
            DT_Illicit_Drugs                    =string.Empty;
            DT_Alcohol                          =string.Empty;
            DT_Amphetamines                     =string.Empty;
            DT_Barbiturates                     =string.Empty;
            DT_Benzodiazepines                  =string.Empty;
            DT_Cocaine1                         =string.Empty;
            DT_Heroin                           =string.Empty;
            DT_Ketamine                         =string.Empty;
            DT_LSD                              =string.Empty;
            DT_Marijuana1                       =string.Empty;
            DT_MDMA                             =string.Empty;
            DT_Opiates1                         =string.Empty;
            DT_Rohypnol                         =string.Empty;
            DT_Steroids                         =string.Empty;
            DT_Synthetic_Drugs                  =string.Empty;
            DT_benzoes                          =string.Empty;
            DT_BP_Test                          =string.Empty;
            DT_Dehydroepiandrosterone           =string.Empty;
            DT_Ecstacy                          =string.Empty;
            DT_Epitestosterone                  =string.Empty;
            DT_Hydroxyandrosterone              =string.Empty;
            DT_Hydroxyetiocholanolone           =string.Empty;
            DT_Marijuna                         =string.Empty;
            DT_Methamphetamines                 =string.Empty;
            DT_Methandienone                    =string.Empty;
            DT_Nortestosterone                  =string.Empty;
            DT_oxycodon                         =string.Empty;
            DT_Oxymetholone                     =string.Empty;
            DT_pencilin                         =string.Empty;
            DT_Phencyclidine1                   =string.Empty;
            DT_Any_Other_Cmmnts                 =string.Empty;
            DT_Others                           =string.Empty;
            DT_Others2                          =string.Empty;
            DT_Others3                          =string.Empty;
            DT_Others4                          =string.Empty;
            DT_Others5                          =string.Empty;
            DT_Others6                          =string.Empty;
            DT_Others7                          =string.Empty;
            ATA_Reverse_Directory               =string.Empty;
            ATA_Residcl_Commrcl                 =string.Empty;
            ATA_Stock_Exchnge                   =string.Empty;
            ATA_Telphne_Directry_Srch           =string.Empty;
            ATA_Yellow_Pages                    =string.Empty;
            ATA_Who_Domain                      =string.Empty;
            ATA_GoogleSearch                    =string.Empty;
            ATA_InfrastructureOfCmp             =string.Empty;
            ATA_Just_Dial                       =string.Empty;
            ATA_Neighbor_Check1                 =string.Empty;
            ATA_NASCOM_Empanelment              =string.Empty;
            ATA_Photograph                      =string.Empty;
            ATA_Physical_Site                   =string.Empty;
            ATA_Reg_Company                     =string.Empty;
            ATA_PO_Courier_Check                =string.Empty;
            ATA_Network_Sol                     =string.Empty;
            ATA_ApprxEmpWorking                 =string.Empty;
            ATA_CID_No                          =string.Empty;
            ATA_Cmpny_Addr                      =string.Empty;
            ATA_Decoy_Call                      =string.Empty;
            ATA_Cmpny_Website                   =string.Empty;
            DT_OtherProof = string.Empty;

            TypeRevert = string.Empty;
            CheckStatus = string.Empty;
            CaseStatus = string.Empty;
            ColorName = string.Empty;
            Remarks = string.Empty;
            Status = 0;

            VerifiedBy = 0;
            VerifierDesignation = string.Empty;
            VerifierContactNo = string.Empty;
            VerifierMobileNo = string.Empty;
            VerifierEmailId = string.Empty;
        }

public int          DrugTestVerRouID { get; set; }
public int          DrugTestRowID       { get; set; }
public virtual PQDrugTest PQDrugTest { get; set; }
public string DT_Cand_Name                      {get;set;}// Text Filed 
public string DT_Amphetamine                    {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Barbiturate                    {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Benzodiazepine                 {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Cannabinoids                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Cocaine                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Date_Verf                      {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Marijuana                      {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Marijuana_Cannabinoids         {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Methadone                      {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Methamphetamine                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Methaqualone                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Morphine_Opiate                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Opiates                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Oxycodone                      {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Phencyclidine                  {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Propoxyphene                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Tricyclic_Antidepressant       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Illicit_Drugs                  {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Alcohol                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Amphetamines                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Barbiturates                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Benzodiazepines                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Cocaine1                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Heroin                         {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Ketamine                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_LSD                            {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Marijuana1                     {get;set;}//Drop Down- Positive / Negative / Others
public string DT_MDMA                           {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Opiates1                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Rohypnol                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Steroids                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Synthetic_Drugs                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_benzoes                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_BP_Test                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Dehydroepiandrosterone         {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Ecstacy                        {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Epitestosterone                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Hydroxyandrosterone            {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Hydroxyetiocholanolone         {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Marijuna                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Methamphetamines               {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Methandienone                  {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Nortestosterone                {get;set;}//Drop Down- Positive / Negative / Others
public string DT_oxycodon                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Oxymetholone                   {get;set;}//Drop Down- Positive / Negative / Others
public string DT_pencilin                       {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Phencyclidine1                 {get;set;}//Drop Down- Positive / Negative / Others
public string DT_Any_Other_Cmmnts               {get;set;}//text Area
public string DT_Others                         {get;set;}//text Area
public string DT_Others2                        {get;set;}//text Area
public string DT_Others3                        {get;set;}//text Area
public string DT_Others4                        {get;set;}//text Area
public string DT_Others5                        {get;set;}//text Area
public string DT_Others6                        {get;set;}//text Area
public string DT_Others7                        {get;set;}//text Area
public string ATA_Reverse_Directory             {get;set;}// Text Area 
public string ATA_Residcl_Commrcl               {get;set;}// Text Area 
public string ATA_Stock_Exchnge                 {get;set;}// Text Area 
public string ATA_Telphne_Directry_Srch         {get;set;}// Text Area 
public string ATA_Yellow_Pages                  {get;set;}// Text Area 
public string ATA_Who_Domain                    {get;set;}// Text Area 
public string ATA_GoogleSearch                  {get;set;}// Text Area 
public string ATA_InfrastructureOfCmp           {get;set;}// Text Area 
public string ATA_Just_Dial                     {get;set;}// Text Area 
public string ATA_Neighbor_Check1               {get;set;}// Text Area 
public string ATA_NASCOM_Empanelment            {get;set;}// Text Area 
public string ATA_Photograph                    {get;set;}// Text Area 
public string ATA_Physical_Site                 {get;set;}// Text Area 
public string ATA_Reg_Company                   {get;set;}// Text Area 
public string ATA_PO_Courier_Check              {get;set;}// Text Area 
public string ATA_Network_Sol                   {get;set;}// Text Area 
public string ATA_ApprxEmpWorking               {get;set;}// Text Area 
public string ATA_CID_No                        {get;set;}// Text Filed 
public string ATA_Cmpny_Addr                    {get;set;}// Text Filed 
public string ATA_Decoy_Call                    {get;set;}// Text Area 
public string ATA_Cmpny_Website                 {get;set;}// Text Area 
public string DT_OtherProof                     { get; set; }  // Text Area 


        public string TypeRevert { get; set; }
        public string CheckStatus { get; set; }
        public string CaseStatus { get; set; }
        public string ColorName { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
