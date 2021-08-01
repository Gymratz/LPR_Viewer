﻿USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_AllPlates]    Script Date: 7/31/2021 9:43:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_LPR_AllPlates]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@Plate nvarchar(50),
	@HideNeighbors bit = 0,
	@CurrentOffset varchar(10) = '-07:00',
	@IdentifyDupes int = 0,
	@TopPH int = 999,
	@Status nvarchar(100) = '%',
	@Camera nvarchar(50) = '%',
	@Desc nvarchar(50) = '%',
	@Make nvarchar(50) = '%',
	@Model nvarchar(50) = '%',
	@Color nvarchar(50) = '%',
	@VIN nvarchar(50) = '%'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Top (@TopPH)
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) as [Local Time],
		PH.best_plate as [Plate],
		Case
			When PH.direction_of_travel_degrees <= 22.5 Then N'⬆'
			When PH.direction_of_travel_degrees <= 67.5 Then N'⬈'
			When PH.direction_of_travel_degrees <= 112.5 Then N'→'
			When PH.direction_of_travel_degrees <= 157.5 Then N'⬊'
			When PH.direction_of_travel_degrees <= 202.5 Then N'⬇'
			When PH.direction_of_travel_degrees <= 247.5 Then N'⬋'
			When PH.direction_of_travel_degrees <= 292.5 Then N'←'
			When PH.direction_of_travel_degrees <= 337.5 Then N'⬉'
			Else N'⬆'
		End as [D],
		KP.Description,
		PH.region as [Region],
		PH.vehicle_color as [Color],
		PH.vehicle_make as [Make],
		PH.vehicle_make_model as [Model],
		PH.vehicle_body_type as [Body],
		PH.best_uuid as [Picture],
		(
			Select
				Count(PHinner.best_plate)
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) > dateadd(hour, -24, GetDate()) AND
				PHTHinner.reason is NULL
		) as [Hits Day],
		(
			Select
				Count(PHinner.best_plate)
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) > dateadd(day, -7, GetDate()) AND
				PHTHinner.reason is NULL
		) as [Hits Week],
		(
			Select
				Count(Distinct CONVERT(date, Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime)))
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				PHTHinner.reason is NULL		
		) as [Distinct Days],
		KP.Status,
		PH.plate_x1, 
		PH.plate_x2, 
		PH.plate_x3, 
		PH.plate_x4,
		PH.plate_y1,
		PH.plate_y2,
		PH.plate_y3,
		PH.plate_y4,
		PH.vehicle_region_height,
		PH.vehicle_region_width,
		PH.vehicle_region_x,
		PH.vehicle_region_y,
		PH.pk,
		KP.Alert_Address,
		LPRAC.vin as [VIN],
		LPRAC.year as [Yr],
		LPRAC.Color as [Car Color],
		LPRAC.make as [Car Make],
		LPRAC.model as [Car Model],
		LocalInfo.best_color as [ALPR Color],
		LocalInfo.best_model as [ALPR Model]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_AutoCheck as LPRAC on LPRAC.Plate = PH.best_plate
	Left Join LPR_LocalInfo as LocalInfo on LocalInfo.UUID = PH.best_uuid
	Where
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) between @StartDate and @EndDate AND
		(@HideNeighbors = 0 OR IsNull(KP.Status, '') <> 'Neighbor') AND
		(@IdentifyDupes = 0 OR
		(
			Select
				Count(*)
			From LPR_PlateHits as PHDup
			Left Join LPR_PlateHits_ToHide as PHTHDup on PHTHDup.pk = PHDup.pk
			Where
				PHDup.best_plate = PH.best_plate AND
				datediff(second, PH.epoch_time_end, PHDup.epoch_time_end) between -30 and 30 AND
				PHDup.pk <> PH.pk AND
				PHTHDup.reason is NULL
		) >= @IdentifyDupes) AND
		(@Status = '%' OR IsNull(KP.Status, '') like @Status) AND
		(@Camera = '%' OR IsNull(PH.camera, '') like @Camera) AND
		(@Plate = '%' OR PH.best_plate like @Plate) AND
		(@Desc = '%' OR KP.Description like @Desc) AND
		(@Color = '%' OR LPRAC.Color like @Color OR LocalInfo.best_color like @Color) AND
		(@Make = '%' OR LPRAC.Make like @Make OR LocalInfo.best_make like @Make) AND
		(@Model = '%' OR LPRAC.model like @Model OR LocalInfo.best_model like @Model) AND
		(@VIN = '%' OR LPRAC.vin like @VIN) AND
		(@Plate <> '%' OR PH.pk not in (Select PHTH.pk from LPR_PlateHits_ToHide as PHTH))
	Order By
		PH.epoch_time_end Desc
END
GO


