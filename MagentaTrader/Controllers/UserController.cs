﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Security;
using System.Net.Http;
using System.Web.Http;

namespace MagentaTrader.Controllers
{
    public class UserController : ApiController
    {
        private Data.MagentaTradersDBDataContext db = new Data.MagentaTradersDBDataContext();

        // GET api/User
        [Authorize]
        public List<Models.User> Get()
        {
            var retryCounter = 0;
            List<Models.User> values;

            while (true)
            {
                try
                {
                    var Users = from d in db.MstUsers
                                where d.AspNetUserId != null
                                select new Models.User
                                {
                                    Id = d.Id,
                                    UserName = d.UserName,
                                    FirstName = d.FirstName,
                                    LastName = d.LastName,
                                    EmailAddress = d.EmailAddress,
                                    PhoneNumber = d.PhoneNumber
                                };
                    if (Users.Count() > 0)
                    {
                        values = Users.ToList();
                    }
                    else
                    {
                        values = new List<Models.User>();
                    }
                    break;
                }
                catch
                {
                    if (retryCounter == 3)
                    {
                        values = new List<Models.User>();
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                    retryCounter++;
                }
            }
            return values;
        }

        // POST api/AddUser
        [Authorize]
        [Route("api/AddUser")]
        public int Post(Models.User value)
        {
            try
            {

                Data.MstUser NewUser = new Data.MstUser();

                NewUser.UserName = value.UserName;
                NewUser.FirstName = value.FirstName;
                NewUser.LastName = value.LastName;
                NewUser.EmailAddress = value.EmailAddress;
                NewUser.PhoneNumber = value.PhoneNumber;

                db.MstUsers.InsertOnSubmit(NewUser);
                db.SubmitChanges();

                return NewUser.Id;
            }
            catch
            {
                return 0;
            }
        }

        // PUT /api/UpdateUser/5
        [Authorize]
        [Route("api/UpdateUser/{Id}")]
        public HttpResponseMessage Put(String Id, Models.User value)
        {
            Id = Id.Replace(",", "");
            int id = Convert.ToInt32(Id);

            try
            {
                var Users = from d in db.MstUsers where d.Id == id select d;
                
                if (Users.Any())
                {
                    var UpdatedUser = Users.FirstOrDefault();

                    UpdatedUser.UserName = value.UserName;
                    UpdatedUser.FirstName = value.FirstName;
                    UpdatedUser.LastName = value.LastName;
                    UpdatedUser.EmailAddress = value.EmailAddress;
                    UpdatedUser.PhoneNumber = value.PhoneNumber;

                    db.SubmitChanges();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (NullReferenceException)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/DeleteUser/5
        [Authorize]
        [Route("api/DeleteUser/{Id}")]
        public HttpResponseMessage Delete(int Id)
        {
            Data.MstUser DeleteUser = db.MstUsers.Where(d => d.Id == Id).First();

            if (DeleteUser != null)
            {
                db.MstUsers.DeleteOnSubmit(DeleteUser);
                try
                {
                    db.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // GET api/GetUser/dpilger
        [Authorize]
        [Route("api/GetUser/{username}")]
        public List<Models.User> GetUserInfo( String UserName )
        {
            //List<Models.User> UserInfo = null;
            //var Info = from m in db.MstUsers
            //           where m.UserName == username
            //           select new Models.User
            //           {
            //               UserName = m.UserName,
            //               FirstName = m.FirstName,
            //               LastName = m.LastName,
            //               EmailAddress = m.EmailAddress,
            //               PhoneNumber = m.PhoneNumber
            //           };

            //UserInfo = Info.ToList();
            //return UserInfo;
            var retryCounter = 0;
            List<Models.User> values;

            while (true)
            {
                try
                {
                    var Info = from m in db.MstUsers
                               where m.UserName == UserName
                               select new Models.User
                               {
                                   Id = m.Id,
                                   UserName = m.UserName,
                                   FirstName = m.FirstName,
                                   LastName = m.LastName,
                                   EmailAddress = m.EmailAddress,
                                   PhoneNumber = m.PhoneNumber
                               };
                    if (Info.Count() > 0)
                    {
                        values = Info.ToList();
                    }
                    else
                    {
                        values = new List<Models.User>();
                    }
                    break;
                }
                catch
                {
                    if (retryCounter == 3)
                    {
                        values = new List<Models.User>();
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                    retryCounter++;
                }
            }
            return values;
        }
    }
}
