======= View Model =========
====== Customer List =======

public List<CustomerViewModel> GetCustomerList()
        {
            using (var db = new SNJewellerEntities())
            {
                List<CustomerViewModel> lstCustomer = new List<CustomerViewModel>();
                var CustomerList = db.tblCustomer.Where(a => a.IsDeleted != true).ToList();
                foreach(var item in CustomerList)
                {
                    lstCustomer.Add(new CustomerViewModel
                    {
                        CustomerID = item.CustomerID,
                        ProfilePhoto = item.ProfilePhoto,
                        FullName = item.FirstName + " " + item.LastName,
                        Email = item.Email,
                        MobileNumber = item.MobileNumber,
                        OTP = item.OTP,
                        Status = item.Status,
                        SubscriptionID = item.SubscriptionID,
                        SubscriptionName = item.SubscriptionID > 0 ? db.tblSubscription.Find(item.SubscriptionID).SubscriptionName : "Default"
                    });
                }
                return lstCustomer;
            }
        }


========= Add Customer =========

   public bool CreateCustomer(CustomerViewModel model)
        {
            using (var db = new SNJewellerEntities())
            {
                if (model.CustomerID == 0)
                {
                    tblCustomer objData = new tblCustomer();
                    objData.FirstName = model.FirstName;
                    objData.LastName = model.LastName;
                    objData.Email = model.Email;
                    objData.PhoneNumber = model.PhoneNumber;
                    objData.MobileNumber = model.MobileNumber;
                    objData.OTP = model.OTP;
                    objData.SubscriptionID = db.tblSubscription.Find(1).SubscriptionID > 0 ? Convert.ToInt32(db.tblSubscription.Find(1).SubscriptionID) : 0;
                    objData.AddedBy = model.AddedBy;
                    objData.AddedDate = DateTime.Now;
                    objData.IsDeleted = false;
                    objData.ProfilePhoto = model.ProfilePhoto;
                    objData.Password = model.Password;

                    db.tblCustomer.Add(objData);
                }
                else
                {
                    tblCustomer objData = db.tblCustomer.Find(model.CustomerID);
                    objData.FirstName = model.FirstName;
                    objData.LastName = model.LastName;
                    objData.Email = model.Email;
                    objData.PhoneNumber = model.PhoneNumber;
                    objData.MobileNumber = model.MobileNumber;
                    if (model.ProfilePhoto != null)
                        objData.ProfilePhoto = model.ProfilePhoto;
                    objData.UpdateBy = model.UpdateBy;
                    objData.UpdateDate = DateTime.Now;
                }

                if (db.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }


========= Delete Customer ===========

 public bool DeleteCustomer(decimal id, decimal userID)
        {
            using (var db = new SNJewellerEntities())
            {
                if (id > 0)
                {
                    tblCustomer objData = db.tblCustomer.Find(id);
                    objData.IsDeleted = true;
                    objData.DeletedBy = userID;
                    objData.DeletedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }


========== Controller ============

  public ActionResult Index()
        {
            try
            {
                CustomerViewModel model = new CustomerViewModel();
                model.lstCustomerList = model.GetCustomerList();
                model.lstCustomerList = model.lstCustomerList.OrderByDescending(a => a.CustomerID).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                ErrorHelper.GetErrorObject().Capture(new SentryEvent(ex));
                return RedirectToAction("Index", "Error");
            }
        }


============== Open Add form ===============
	[HttpGet]
        public ActionResult AddCustomer(decimal? id)
        {
            try
            {
                CustomerViewModel model = new CustomerViewModel();
                if (id > 0)
                {
                    model = model.getCustomerData(id.Value);
                }
                return PartialView("_AddCustomer", model);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                ErrorHelper.GetErrorObject().Capture(new SentryEvent(ex));
                return RedirectToAction("Index", "Error");
            }
        }


=========== Add Customer ===============


        [HttpPost]
        public ActionResult AddCustomer(CustomerViewModel model)
        {
            try
            {
                var userID = SessionManager.GetUserID();
                model = new JavaScriptSerializer().Deserialize<CustomerViewModel>(Request.Form["CustomerViewModel"]);
                model.CustomerPicsAttachment = Request.Files["CustomerPicsAttachment"];
                model.AddedBy = userID;
                model.UpdateBy = userID;
                if (model.isEmailexist(model.Email, model.CustomerID))
                {
                    var profileImage = SaveToServer(model.CustomerPicsAttachment);
                    if (profileImage != null)
                        model.ProfilePhoto = profileImage;
                    model.CustomerPicsAttachment = null;
                    model.OTP = MailHelper.CreateOTP();
                    model.Password = SNJGlobal.createRandomPassword(8);
                    var result = model.CreateCustomer(model);
                    if (result == true && model.CustomerID == 0)
                    {
                        if (ConfigHelper.ISSendMessage == true)
                        {
                            ////OTP sent to user through message
                            var strMessage = SMSHelper.sendSMS(model.MobileNumber, model.FirstName, Constant.ACTIVATIONCODE.Replace("<<code>>", model.OTP));
                        }
                        ////OTP sent to user through mail
                        string strOTP = MailHelper.ReadFile("OTPTemplate.html");
                        strOTP = strOTP.Replace("{username}", model.FirstName + " " + model.LastName);
                        strOTP = strOTP.Replace("{otp}", model.OTP);
                        new Thread(() => { MailHelper.sendMail("User registration OTP", strOTP, model.Email); }).Start();


                        ////For UserName and password in message
                        var Message = Constant.USERNAMEANDPASSWORD;
                        Message = Message.Replace("<<username>>", model.Email);
                        Message = Message.Replace("<<password>>", model.Password);
                        if (ConfigHelper.ISSendMessage == true)
                        {
                            ////Username and password sent to user through message
                            var strUsernameAndPasswordMessage = SMSHelper.sendSMS(model.MobileNumber, model.FirstName, Message);
                        }

                        ////Username and password sent to user through mail
                        string strUserNamePass = MailHelper.ReadFile("AuthenticationTemplate.html");
                        strUserNamePass = strUserNamePass.Replace("{username}", model.FirstName + " " + model.LastName);
                        strUserNamePass = strUserNamePass.Replace("{userId}", model.Email);
                        strUserNamePass = strUserNamePass.Replace("{password}", model.Password);
                        new Thread(() => { MailHelper.sendMail("SN Jewellery Credentials", strUserNamePass, model.Email); }).Start();


                        ////Notify Admin on new customer registration
                        var NotificationID = 0.0M;
                        using (var db = new SNJewellerEntities())
                        {
                            NotificationID = db.tblNotification.Where(x => x.NotificationName == Constant.ACTIVITYCUSTOMERCREATE).Select(x => x.NotificationID).FirstOrDefault();
                        }
                        if (NotificationID > 0)
                        {
                            var EmailIDs = MailHelper.lstAdminUsersEmail(NotificationID);
                            foreach (var item in EmailIDs)
                            {
                                if (item != null)
                                {
                                    using (var db = new SNJewellerEntities())
                                    {
                                        var UserName = db.tblUser.Where(x => x.Email == item).FirstOrDefault();
                                        var Fullname = UserName.FirstName + " " + UserName.LastName;
                                        string strNewCutomerAdmin = MailHelper.ReadFile("NewCustomerAdmin.html");
                                        strNewCutomerAdmin = strNewCutomerAdmin.Replace("{username}", Fullname);
                                        strNewCutomerAdmin = strNewCutomerAdmin.Replace("{userId}", model.Email);
                                        strNewCutomerAdmin = strNewCutomerAdmin.Replace("{fullname}", model.FirstName + " " + model.LastName);
                                        strNewCutomerAdmin = strNewCutomerAdmin.Replace("{mobile}", model.MobileNumber);
                                        new Thread(() => { MailHelper.sendMail("New Customer", strNewCutomerAdmin, item); }).Start();
                                    }

                                }
                            }

                            if (ConfigHelper.ISSendMessageToAdmin == true)
                            {
                                var MobielNos = SMSHelper.lstAdminUsersMobileNo(NotificationID);
                                foreach (var item in MobielNos)
                                {
                                    if (item != null)
                                    {
                                        using (var db = new SNJewellerEntities())
                                        {
                                            var TextMessage = Constant.NEWCUSTOMERTOADMIN;
                                            var UserName = db.tblUser.Where(x => x.MobileNumber == item).FirstOrDefault();
                                            var Fullname = UserName.FirstName + " " + UserName.LastName;
                                            TextMessage = TextMessage.Replace("<<username>>", Fullname);
                                            TextMessage = TextMessage.Replace("<<customername>>", model.FirstName + " " + model.LastName);
                                            if (ConfigHelper.ISSendMessage == true)
                                            {
                                                ////New Customer to notification to admin users
                                                var strNewCustomerToAdmin = SMSHelper.sendSMS(item, model.FirstName, TextMessage);

                                            }
                                        }

                                    }
                                }
                            }
                        }

                        return Json(new { Msg = "Success" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                    return Json(new { Msg = "EmailExist" }, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                ErrorHelper.GetErrorObject().Capture(new SentryEvent(ex));
                return RedirectToAction("Index", "Error");
            }
        }



============= Delete Customer ==============

  public ActionResult Delete(decimal id)
        {
            try
            {
                var userID = SessionManager.GetUserID();
                CustomerViewModel model = new CustomerViewModel();
                if (id > 0)
                {
                    var result = model.DeleteCustomer(id, userID);
                }
                return Json(new { Msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError(ex);
                ErrorHelper.GetErrorObject().Capture(new SentryEvent(ex));
                return RedirectToAction("Index", "Error");
            }
        }



======= Add Button ======

<a href="javascript:;" class="btn" title="Add New" style="float:right;margin-top:-25px" onclick="OpenAddCustomer()">Add New</a>


function OpenAddCustomer() {
        $("#dvLoading").show();
        $.ajax({
            url: "/Customer/AddCustomer/",
        }).done(function (e) {
            $("#dvLoading").hide();
            if (e.error) {
                alert(e.error);
            }
            else {
                $("#divAddEdit").html(e);
                $("#dialogAddEdit").modal('show');
                centerModals();
            }
        });
    }


========= _AddCustomer ==================

@model SNJewellerDAL.ViewModel.CustomerViewModel
@using System.Configuration
<script src="~/Scripts/jquery.validate.min.js"></script>
<script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
<div class="modal fade" id="dialogAddEdit" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    @*@using (Ajax.BeginForm("AddDiamondType", "DiamondType", new AjaxOptions
        {
            HttpMethod = "POST",
            //UpdateTargetId = "divList",
            //OnBegin = "return onClickcheckValidation();",
            LoadingElementId = "divLoading",
            OnSuccess = "$('#dialogAddEdit').modal('hide')",
            OnFailure = "$('#dialogAddEdit').modal('show');"
        }, new { @class = "form-horizontal" }))
        {*@
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><i class="pixie-apt-close"></i></button>
                <h4 class="modal-title" id="myModalLabel"> @(Model.CustomerID != 0 && Model.CustomerID != null ? "Edit Customer" : "Add Customer")</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            @Html.HiddenFor(m => m.CustomerID)
                            <label>First Name</label>
                            @Html.TextBoxFor(m => m.FirstName, new { @class = "form-control", @placeholder = "First Name" })
                            @Html.ValidationMessageFor(m => m.FirstName, "", new { @class = "text-danger identity clsFirstName",@style="font-weight:bold;color:red;font-size:13px" })
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Last Name</label>
                            @Html.TextBoxFor(m => m.LastName, new { @class = "form-control", @placeholder = "Last Name" })
                            @Html.ValidationMessageFor(m => m.LastName, "", new { @class = "text-danger identity clsLastName", @style = "font-weight:bold;color:red;font-size:13px" })
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Email</label>
                            @Html.TextBoxFor(m => m.Email, new { @class = "form-control", @placeholder = "Email" })
                            @Html.ValidationMessageFor(m => m.Email, "", new { @class = "text-danger identity clsEmail", @style = "font-weight:bold;color:red;font-size:13px" })
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Mobile</label>
                            @Html.TextBoxFor(m => m.MobileNumber, new { @class = "form-control", @placeholder = "MobileNumber" })
                            @Html.ValidationMessageFor(m => m.MobileNumber, "", new { @class = "text-danger identity clsMobileNumber", @style = "font-weight:bold;color:red;font-size:13px" })
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Phone</label>
                            @Html.TextBoxFor(m => m.PhoneNumber, new { @class = "form-control", @placeholder = "PhoneNumber" })
                            @Html.ValidationMessageFor(m => m.PhoneNumber, "", new { @class = "text-danger identity clsPhoneNumber", @style = "font-weight:bold;color:red;font-size:13px" })
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            @if (Model.ProfilePhoto != null)
                            {
                                var folderName = ConfigurationManager.AppSettings["CustomerImageFolderName"].ToString();
                                @Html.HiddenFor(m => m.ProfilePhoto)
                                <img id="imgLogo" title="imgcustomerimage" src="~/SNJewellersImages/@folderName/@Model.ProfilePhoto" height="125" width="125" class="img-circle" style="margin-bottom: 2%" />
                            }
                            else
                            {
                                @*<img id="imgLogo" title="imgcustomerimage" src="" height="125" width="125" class="img-circle" />*@
                                <img id="imgLogo" title="imgcustomerimage" src="~/images/noimage.png" height="125" width="125" class="img-thumbnail" style="margin-bottom:3%" />
                            }
                            @Html.TextBoxFor(model => model.CustomerPicsAttachment, new { @class = "form-control clsCustomerPicsAttachment", @type = "file", @id = "myImage" })
                            <span id="Error" class="text-danger field-validation-error clsCustomerImage" style="display: block; margin-top: 0px;"></span>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="modal-btn">
                        <br />
                        <button class="btn common-btn" id="btnSubmit" type="submit" onclick="AddCustomer()">Submit</button>
                        <button class="btn common-btn cacel" data-dismiss="modal" type="button">cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

=========================================



============= AddCustomer() =============

    function AddCustomer() {

        var CustomerID = $("#CustomerID").val();
        var FirstName = $("#FirstName").val();
        var LastName = $("#LastName").val();
        var Email = $("#Email").val();
        var MobileNumber = $("#MobileNumber").val();
        var PhoneNumber = $("#PhoneNumber").val();
        var File = $(".clsCustomerPicsAttachment")[0].files[0];

        var flag = false;
        var emailval = /^\w+([\.-]?\w+)*@@\w+([\.-]?\w+)*(\.\w{2,15})+$/;
        var exp = /^[a-zA-Z0-9 ,./&]*$/
        var phoneexp = /^[0-9-+]*$/

        if (FirstName == "") {
            $(".clsFirstName").html("Please Enter First Name.");
            return flag;
        } else {
            if (!exp.test(FirstName)) {
                $(".clsFirstName").html("Special Character Not Allowed.");
                return flag;
            } else
                $(".clsFirstName").html("");
        }

        if (FirstName != "") {
            if (FirstName.length > 50) {
                $(".clsFirstName").html("Max 50 characters.");
                return flag;
            } else
                $(".clsFirstName").html("");
        }

        if (LastName == "") {
            $(".clsLastName").html("Please Enter Last Name.");
            return flag;
        } else {
            if (!exp.test(LastName)) {
                $(".clsLastName").html("Special Character Not Allowed.");
                return flag;
            } else
                $(".clsLastName").html("");
        }

        if (LastName != "") {
            if (LastName.length > 50) {
                $(".clsLastName").html("Max 50 characters.");
                return flag;
            } else
                $(".clsLastName").html("");
        }

        if (Email == "") {
            $(".clsEmail").html("Please Enter Email.");
            return flag;
        } else {
            if (!emailval.test(Email)) {
                $(".clsEmail").html("Invalid Email.");
                return flag;
            }
            else
                $(".clsEmail").html("");
        }

        if (MobileNumber == "") {
            $(".clsMobileNumber").html("Please Enter Mobile Number.");
            return flag;
        } else {
            if (!phoneexp.test(MobileNumber)) {
                $(".clsMobileNumber").html("Invalid Mobile Number.");
                return flag;
            } else
                $(".clsMobileNumber").html("");
        }

        if (MobileNumber != "") {
            if (MobileNumber.length > 13) {
                $(".clsMobileNumber").html("Invalid Mobile Number.");
                return flag;
            } else {
                if (MobileNumber.length < 10) {
                    $(".clsMobileNumber").html("Invalid Mobile Number.");
                    return flag;
                }
                $(".clsMobileNumber").html("");
            }
        }


        if (PhoneNumber != "") {
            if (!phoneexp.test(PhoneNumber)) {
                $(".clsPhoneNumber").html("Invalid Phone Number.");
                return flag;
            } else
                $(".clsPhoneNumber").html("");
        }

        if (PhoneNumber != "") {
            if (PhoneNumber.length > 13) {
                $(".clsPhoneNumber").html("Invalid Phone Number.");
                return flag;
            } else
                $(".clsPhoneNumber").html("");
        }

        if (flag)
            return false;


        var model = {
            "CustomerID": CustomerID,
            "FirstName": FirstName,
            "LastName": LastName,
            "Email": Email,
            "MobileNumber": MobileNumber,
            "PhoneNumber": PhoneNumber
        }

        var data = new FormData();
        data.append("CustomerViewModel", JSON.stringify(model));
        data.append("CustomerPicsAttachment", File);
        $("#dvLoading").show();
        $.ajax({
            url: "/Customer/AddCustomer",
            type: 'POST',
            contentType: false,
            processData: false,
            data: data
        }).done(function (e) {
            $("#dvLoading").hide();
            $("#divAddEdit").html(e);
            if (e.Msg == "EmailExist") {
                $(".clsEmail").html("Email Already Exist");
                return false;
            }
            if (e.Msg == "Success") {
                $("#dialogAddEdit").modal('hide');
                if (CustomerID != 0)
                    onUpdateSuccessfully();
                else
                    onAddSuccessfully();
            }
        });
    }

========================================

  function openEditCustomer(obj) {
        var id = obj.getAttribute('data-id');
        $("#dvLoading").show();
        $.ajax({
            url: "/Customer/AddCustomer/" + id,
        }).done(function (e) {
            $("#dvLoading").hide();
            if (e.error) {
                alert(e.error);
            }
            else {
                $("#divAddEdit").html(e);
                $("#dialogAddEdit").modal('show');
                centerModals();
            }
        });
    }




================= Modal Pop-Up ======================


<div class="modal fade medium-box-popup in" id="idDialogBoxDelete" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Delete Customer</h4>
            </div>
            <div class="modal-body">
                <h3 style="display:block">Are you sure want to delete this customer?</h3>
                <input type="submit" class="btn clsDeleteCustomer" style="display:inline-block" value="Delete" onclick="ConfirmDelete(this)" />
                <button class="btn" style="display:inline-block" data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade small-box-popup" id="RecordDelete" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><i class="pixie-apt-close"></i></button>
                <h4 class="modal-title" id="myModalLabel">Success</h4>
            </div>
            <div class="modal-body" id="bodyMessage">
                <h3>Record deleted successfully !</h3>
                <button class="btn" style="display:inline-block" data-dismiss="modal">Ok</button>
            </div>
        </div>
    </div>
</div>

===================================================================================

<a href="javascript:;" onclick="openEditCustomer(this)" data-id="@item.CustomerID"><i class="pixie-apt-edit" title="Edit"></i></a>
<a href="javascript:void(0);" data-id="@item.CustomerID" onclick="OpenDeleteCustomer(this)"><i class="pixie-apt-delete" title="Delete"></i></a>

    function OpenDeleteCustomer(obj) {
        var id = obj.getAttribute("data-id");
        //centerModals();
        //$("#idDialogBoxDelete").modal('show');
        //$(".clsDeleteCustomer").attr("data-id", id);

        swal({
            title: 'Are you sure?',
            text: "Are you sure want to delete this Customer?",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then(function () {
            swal(
                 $(".clsDeleteCustomer").attr("data-id", id),
                ConfirmDelete(obj)

            )
        },
        function (dismiss) {

            if (dismiss === 'cancel') {
                swal(
                )
            }
        })

    }

    function ConfirmDelete(obj) {
        var id = obj.getAttribute("data-id")
        $("#dvLoading").show();
        $.ajax({
            url: "/Customer/Delete/" + id,
            type: 'POST',
        }).done(function (e) {
            $("#dvLoading").hide();
            if (e.error) {
                alert(e.error);
            }
            else {
                if (e.Msg == "Success") {
                    CommonSweetAlert();
                }
            }
        });
    }


