﻿using AutoFixture;
using BussinessLogic;
using Moq;
using Xunit;
using ServiceLayer.Controllers;
using System.Collections.Generic;
using Appointment_DataEntities.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.SqlClient;

namespace Appointment_TestAppointment
{

    public class MoqAppointment
    {
        private readonly IFixture fixture;
        private readonly Mock<IAppointment> _appointment;
        private readonly AppointmentController controller;

        public MoqAppointment()
        {
            fixture = new Fixture();
            _appointment = fixture.Freeze<Mock<IAppointment>>();
            controller = new AppointmentController(_appointment.Object);
        }



        [Fact]
        public void GetAppointmentsByPatient_BadRequest_Test()
        {
            List<Models.Appointment> appointments = null;
            var patient_id = fixture.Create<Guid>();
            _appointment.Setup(x => x.GetAppointmentsByPatient(patient_id)).Throws(new Exception("something went wrong"));

            var result = controller.GetAppointmentByPatientId(patient_id);

            result.Should().BeAssignableTo<BadRequestObjectResult>();

            _appointment.Verify(x => x.GetAppointmentsByPatient(patient_id), Times.AtLeastOnce());
        }

        [Fact]
        public void GetAppointmentsByPatient_OkResponse_Test()
        {
            var appointment = fixture.Create<IEnumerable<Models.Appointment>>();
            var patient_id = fixture.Create<Guid>();
            _appointment.Setup(x => x.GetAppointmentsByPatient(patient_id)).Returns(appointment);

            var result = controller.GetAppointmentByPatientId(patient_id);

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(appointment.GetType());
            _appointment.Verify(x => x.GetAppointmentsByPatient(patient_id), Times.AtLeastOnce());

        }
        [Fact]
        public void GetAppointmentsByPatient_NoContent()
        {
            IEnumerable<Models.Appointment> appointments = null;
            var patient_id=fixture.Create<Guid>();
            _appointment.Setup(x => x.GetAppointmentsByPatient(patient_id)).Returns(appointments);

            var res=controller.GetAppointmentByPatientId(patient_id);
            res.Should().BeAssignableTo<NoContentResult>();
           
        }

      
        [Fact]
        public void GetAppointmentByDate_Test()
        {
            var appointmentMock= fixture.Create<IEnumerable<Models.Appointment>>();
            var app = fixture.Create<Models.Appointment>();

            var date=fixture.Create<DateTime>();
            string dt = "2023-03-27";
            var date1 = fixture.Create<DateTime>();

            _appointment.Setup(x => x.GetAppointmentsByDate(date1)).Returns(appointmentMock);
            var result = controller.GetAppointmentByDate(dt);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
         

        }
        [Fact]
        public void GetAppointmentByDate_BadRequest_Test()
        {
            IEnumerable<Models.Appointment> appointments = null;
            var date = fixture.Create<string>();
            var date1 = fixture.Create<DateTime>();
            string dt = "2023-03-27";
            _appointment.Setup(x => x.GetAppointmentsByDate(date1)).Returns(appointments);

            var result = controller.GetAppointmentByDate(date);

           
            result.Should().BeAssignableTo<BadRequestObjectResult>();

           
        }
        [Fact]
        public void GetAppDateBAd()
        {
            var appointmentMock = fixture.Create<IEnumerable<Models.Appointment>>();
            var app = fixture.Create<Models.Appointment>();

            var date = fixture.Create<DateTime>();
            string dt = "2023-03-27";
            var date1 = fixture.Create<DateTime>();

            _appointment.Setup(x => x.GetAppointmentsByDate(date1)).Throws(new Exception("Something went wrong"));
            var result = controller.GetAppointmentByDate(dt);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

        }
        [Fact]
        public void GetappAfterBad()
        {
            IEnumerable<Models.Appointment> app = null;
            var docid = fixture.Create<Guid>();
            var date = fixture.Create<string>();
            var date1 = fixture.Create<DateTime>();
            string dt = "2023-04-10";
            _appointment.Setup(x => x.GetAppointmentsAfterCheckup(date1, docid)).Returns(app);
            var res = controller.GetAppointmentsAfterCheckup(docid, date);
            res.Should().BeAssignableTo<BadRequestObjectResult>();
            
        }


        [Fact]
        public void GetAppAfterCatch()
        {
            var appointment = fixture.Create<IEnumerable<Models.Appointment>>();
            var doctor_id = fixture.Create<Guid>();
            var date = fixture.Create<DateTime>();
            string date1 = "2023-03-27";

            _appointment.Setup(x => x.GetAppointmentsAfterCheckup(date, doctor_id)).Throws(new Exception("Something went wrong"));
            var res = controller.GetAppointmentsAfterCheckup(doctor_id, date1);
            res.Should().NotBeNull();
            res.Should().BeAssignableTo<OkObjectResult>();
            //_appointment.Verify(x => x.GetAppointmentsAfterCheckup(date, doctor_id), Times.AtLeastOnce());

        }

        [Fact]
        public void GetAppointmentsAfterCheckup_OkResponse_Test()
        {
            var appointment = fixture.Create<IEnumerable<Models.Appointment>>();
            var doctor_id=fixture.Create<Guid>();
            var date = fixture.Create<DateTime>();
            string date1 = "2023-03-27";


            _appointment.Setup(x=>x.GetAppointmentsAfterCheckup(date,doctor_id)).Returns(appointment);
            var result = controller.GetAppointmentsAfterCheckup(doctor_id, date1);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

        }
        

       


        [Fact]
        public void AddAppointment_OkResponse_Test()
        {
            var app = fixture.Create<Models.Appointment>();
            _appointment.Setup(x => x.AddAppointment(app)).Returns(app);

            var result = controller.AddAppointment(app);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(app.GetType());
            _appointment.Verify(x => x.AddAppointment(app), Times.AtLeastOnce());
        }

        [Fact]
        public void AddAppointment_BadRequest_Test()
        {
            var app1=fixture.Create<Models.Appointment>();
            _appointment.Setup(x => x.AddAppointment(app1)).Throws(new Exception("Something went wrong"));
            var res = controller.AddAppointment(app1);
            res.Should().BeAssignableTo<BadRequestObjectResult>();
            _appointment.Verify(x => x.AddAppointment(app1), Times.AtLeastOnce());


        }
     

        [Fact]
        public void UpdateStatus_Test()
        {
            var appointment1 = fixture.Create<Models.Appointment>();
            var appointment_id = fixture.Create<Guid>();
            var status = fixture.Create<string>();

            _appointment.Setup(x => x.UpdateStatus(appointment_id, status)).Returns(appointment1);
            var result = controller.UpdateStatus(appointment_id, status);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(appointment1.GetType());
            _appointment.Verify(x => x.UpdateStatus(appointment_id, status), Times.AtLeastOnce());
        }
        [Fact]
        public void UpdateStatus_BadRequest_Test()
        {
            List<Models.Appointment> appointments = null;
            var appointment_id = fixture.Create<Guid>();
            var status = fixture.Create<string>();

            _appointment.Setup(x => x.UpdateStatus(appointment_id, status)).Throws(new Exception("something wrong with details"));
            var result = controller.UpdateStatus(appointment_id, status);

            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _appointment.Verify(x => x.UpdateStatus(appointment_id, status), Times.AtLeastOnce());
        }
        [Fact]
        public void UpdateCheckUpStatus()
        {
            var app = fixture.Create<Models.Appointment>();
            var appid = fixture.Create<Guid>();
            var appbool = fixture.Create<bool>();

            _appointment.Setup(s => s.UpdateCheckUpStatus(appid, appbool)).Returns(app);

            var res = controller.UpdateCheckUpStatus(appid, appbool);
            res.Should().NotBeNull();
            res.Should().BeAssignableTo<OkObjectResult>();
            res.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(app.GetType());
            _appointment.Verify(x => x.UpdateCheckUpStatus(appid, appbool), Times.AtLeastOnce());
        }
        [Fact]
        public void UpdateCheckUpStatusCatch()
        {
            var app = fixture.Create<Models.Appointment>();
            var appid = fixture.Create<Guid>();
            var appbool = fixture.Create<bool>();

            _appointment.Setup(x => x.UpdateCheckUpStatus(appid, appbool)).Throws(new Exception("Something went wrong"));

            var res = controller.UpdateCheckUpStatus(appid, appbool);

            res.Should().NotBeNull();
            res.Should().BeAssignableTo<BadRequestObjectResult>();
            _appointment.Verify(x => x.UpdateCheckUpStatus(appid, appbool), Times.AtLeastOnce());
        }


        [Fact]
        public void Email_Notification_Test()
        {
            var app2 = fixture.Create<Models.Appointment>();
           

            var status = fixture.Create<string>();
            var date1 = fixture.Create<string>();
            var email = fixture.Create<string>();
            _appointment.Setup(x => x.EmailFunc(email, date1, status));

            var result = controller.SendEmail(email, date1, status);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }
        [Fact]
        public void emailCatch()
        {
            var app = fixture.Create<Models.Appointment>();
            var email = fixture.Create<string>();
            var status = fixture.Create<string>();
            var date = fixture.Create<string>();
            _appointment.Setup(x => x.EmailFunc(email, date, status)).Throws(new Exception("Soemthing went wong"));
            var res = controller.SendEmail(email, date, status);
            res.Should().BeAssignableTo<BadRequestObjectResult>();
            _appointment.Verify(x => x.EmailFunc(email, date, status), Times.AtLeastOnce());

        }

        [Fact]
        public void GetappointmentsbyDoctoridAndStatus_BadRequest_Test()
        {
            List<Models.Appointment> appointments = null;
            var doctor_id = fixture.Create<Guid>();
            var status = fixture.Create<string>();

            _appointment.Setup(x => x.GetAppointmentsByDoctor_idByStatus(doctor_id, status)).Throws(new Exception("something went wrong"));

            var result = controller.GetAppointmentsByDoctorId(doctor_id, status);

            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _appointment.Verify(x => x.GetAppointmentsByDoctor_idByStatus(doctor_id, status), Times.AtLeastOnce());
        }


        [Fact]
        public void GetappointmentsbyDoctoridAndStatus_OkResponse_Test()
        {
            var appointment=fixture.Create<IEnumerable<Models.Appointment>>();
            var doctor_id=fixture.Create<Guid>();
            var status = fixture.Create<string>();
            _appointment.Setup(x=>x.GetAppointmentsByDoctor_idByStatus(doctor_id,status)).Returns(appointment);

            var result =controller.GetAppointmentsByDoctorId(doctor_id, status);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(appointment.GetType());
            _appointment.Verify(x=>x.GetAppointmentsByDoctor_idByStatus(doctor_id,status), Times.AtLeastOnce());
        }

        [Fact]
        public void GetappointmentsbyDoctoridAndStatus_NoContent_Test()
        {
            List<Models.Appointment> appointments = null;
            var doctor_id = fixture.Create<Guid>();
            var status = fixture.Create<string>();

            _appointment.Setup(x => x.GetAppointmentsByDoctor_idByStatus(doctor_id, status)).Returns(appointments);
            var result = controller.GetAppointmentsByDoctorId(doctor_id, status);

            result.Should().BeAssignableTo<NoContentResult>();
            _appointment.Verify(x => x.GetAppointmentsByDoctor_idByStatus(doctor_id, status), Times.AtLeastOnce());
        }

    }

}
