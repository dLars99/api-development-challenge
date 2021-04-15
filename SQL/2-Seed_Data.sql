USE [PatientLabResults];
GO

set identity_insert [Patient] on
insert into Patient (Id, FirstName, MiddleName, LastName, SexAtBirth, DateOfBirth, Height, Weight, InsuranceCompany, MemberId, GroupId, IsPolicyHolder) values (1, 'David', 'Paul', 'Larsen', 'Male', '01/01/2015', '70', '210', 'Great Big Insurance Co.', '12345678A', '87654321B', 1);
insert into Patient (Id, FirstName, MiddleName, LastName, SexAtBirth, DateOfBirth, Height, Weight, InsuranceCompany, MemberId, GroupId, IsPolicyHolder) values (2, 'Kristina', null, 'Mendoza', 'Female', '01/01/1928', '61', '105', null, null, null, null);
set identity_insert [Patient] off

set identity_insert [LabResult] on
insert into LabResult (Id, TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider, Measurement, MeasurementUnit) values (1, 'Urinalysis', 'normal', 1, '04/03/2019', '04/04/2019', 'Aegis Lab Corp.', 'Dr. John Smith', 23, 'ppm');
insert into LabResult (Id, TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider, Measurement, MeasurementUnit) values (2, 'Blood sugar', 'elevated', 2, '06/02/2016', '06/02/2016', 'Phleb It Shine', 'Raymond Sweets, MD', 160, 'mg/dL');
insert into LabResult (Id, TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider, Measurement, MeasurementUnit) values (3, 'White blood cell count', 'normal', 2, '06/06/2016', '06/10/2016', 'Phleb It Shine', 'Raymond Sweets, MD', 7000, 'WBC/µL');
insert into LabResult (Id, TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider, Measurement, MeasurementUnit) values (4, 'LDL Cholesterol', 'optimal', 1, '08/12/2020', '08/13/2020', 'Renowned Medical Labs Inc.', 'Fran Harding, LNP', 88, 'mg/dL');
insert into LabResult (Id, TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider, Measurement, MeasurementUnit) values (5, 'Transferrin', 'low', 1, '08/12/2020', '08/17/2020', 'Renowned Medical Labs, Inc.', 'Fran Harding, LNP', 185, 'mg/dL');
set identity_insert [LabResult] off