## Script Attachment :
    - Attach BlendedOperation.cs and Bridge.cs to Same GO in which Main_Blended.cs is attached.
    - Create Empty GO Scipt and create follwoing Gamobject and attach script
      - MainBlendedData.cs -> MainBlendedData GO
      - ActivityContentManager.cs -> ActivityContentManager GO
      - QAManager.cs -> QAManager GO
      - ScoreManager.cs -> ScoreManager GO


## Code Changes :
    - Include bool[] 
      - HAS_WORKSHEET, 
      - HAS_SYLLABLE, 
      - HAS_GRAMMER, 
      - HAS_ACTIVITY, 
      - IS_MANUAL_ACTIVITY in Main_Blended.cs if not included
    - 
