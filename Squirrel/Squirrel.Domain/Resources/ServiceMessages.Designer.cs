﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Squirrel.Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ServiceMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ServiceMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Squirrel.Domain.Resources.ServiceMessages", typeof(ServiceMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to گروهی با این نام وجود دارد..
        /// </summary>
        public static string CategoryService_CategoryExisted {
            get {
                return ResourceManager.GetString("CategoryService_CategoryExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to گروهی با این نام وجود ندارد..
        /// </summary>
        public static string CategoryService_CategoryNotFount {
            get {
                return ResourceManager.GetString("CategoryService_CategoryNotFount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این گروه پدر یک یا چند گروه دیگر بوده و قابل حذف نیست..
        /// </summary>
        public static string CategoryService_DeleteAsync_HasChild {
            get {
                return ResourceManager.GetString("CategoryService_DeleteAsync_HasChild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to یک یا چند عنوان با این گروه ارتباط دارند و  گروه مورد نظر قابل حذف نیست..
        /// </summary>
        public static string CategoryService_DeleteAsync_HasTopic {
            get {
                return ResourceManager.GetString("CategoryService_DeleteAsync_HasTopic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to پدری با این نام وجود ندارد..
        /// </summary>
        public static string CategoryService_ParentNotFount {
            get {
                return ResourceManager.GetString("CategoryService_ParentNotFount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نظر مورد نظر یافت نشد..
        /// </summary>
        public static string CommentService_CommentNotFound {
            get {
                return ResourceManager.GetString("CommentService_CommentNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to برای این نظر یک یا چند پاسخ ثبت شده است و قابل حذف نیست..
        /// </summary>
        public static string CommentService_NotRemovable {
            get {
                return ResourceManager.GetString("CommentService_NotRemovable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این تنظیم وجود دارد..
        /// </summary>
        public static string ConfigService_ConfigExisted {
            get {
                return ResourceManager.GetString("ConfigService_ConfigExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to تنظیم مورد نظر یافت نشد..
        /// </summary>
        public static string ConfigService_ConfigNotFound {
            get {
                return ResourceManager.GetString("ConfigService_ConfigNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این پدر برای این گروه قابل انتخاب نیست..
        /// </summary>
        public static string ConfigService_InvalidParent {
            get {
                return ResourceManager.GetString("ConfigService_InvalidParent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to هیچ تنظیماتی برای آدرس ارسال کننده مورد نظر وجود ندارد..
        /// </summary>
        public static string EmailService_NoSmtpSetting {
            get {
                return ResourceManager.GetString("EmailService_NoSmtpSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to در هنگام ارسال ایمیل خطا رخ داد..
        /// </summary>
        public static string EmailService_SendingFailed {
            get {
                return ResourceManager.GetString("EmailService_SendingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to فایل مورد نظر یافت نشد..
        /// </summary>
        public static string FileService_FileNotFount {
            get {
                return ResourceManager.GetString("FileService_FileNotFount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این نوع فایل برای آپلود معتبر نیست..
        /// </summary>
        public static string FileService_InvalidExtension {
            get {
                return ResourceManager.GetString("FileService_InvalidExtension", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شما به این فایل دسترسی ندارید..
        /// </summary>
        public static string FileService_NoAccess {
            get {
                return ResourceManager.GetString("FileService_NoAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to فایل مورد نظر پاک شده است..
        /// </summary>
        public static string FileService_NotExists {
            get {
                return ResourceManager.GetString("FileService_NotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نوع فایل مشخص نیست..
        /// </summary>
        public static string FileService_UnkhownExtension {
            get {
                return ResourceManager.GetString("FileService_UnkhownExtension", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to در هنگام انجام درخواست شما خطایی رخ داد..
        /// </summary>
        public static string General_ErrorAccurred {
            get {
                return ResourceManager.GetString("General_ErrorAccurred", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to اطلاعات ورودی ناقص است..
        /// </summary>
        public static string General_LackOfInputData {
            get {
                return ResourceManager.GetString("General_LackOfInputData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شما دسترسی لازم برای انجام این عملیات را ندارید..
        /// </summary>
        public static string General_NoAccessForThisOp {
            get {
                return ResourceManager.GetString("General_NoAccessForThisOp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to برای دسترسی به این مورد، کاربر باید مشخص باشد..
        /// </summary>
        public static string General_NoUserDefined {
            get {
                return ResourceManager.GetString("General_NoUserDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to لاگ مورد نظر یافت نشد..
        /// </summary>
        public static string LogService_LogNotFound {
            get {
                return ResourceManager.GetString("LogService_LogNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to محتوای پست نمی تواند خالی باشد..
        /// </summary>
        public static string PostService_EmptyPostBody {
            get {
                return ResourceManager.GetString("PostService_EmptyPostBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شما به این پست دسترسی ندارید..
        /// </summary>
        public static string PostService_NoAccess {
            get {
                return ResourceManager.GetString("PostService_NoAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to پست مورد نظر یافت نشد..
        /// </summary>
        public static string PostService_PostNotFound {
            get {
                return ResourceManager.GetString("PostService_PostNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to هیچ پروفایلی یافت نشد..
        /// </summary>
        public static string ProfileService_ProfileNotFound {
            get {
                return ResourceManager.GetString("ProfileService_ProfileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این کاربر پروفایل ندارد..
        /// </summary>
        public static string ProfileService_UserHasNotProfile {
            get {
                return ResourceManager.GetString("ProfileService_UserHasNotProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این کاربر پروفایل دارد..
        /// </summary>
        public static string ProfileService_UserHasProfile {
            get {
                return ResourceManager.GetString("ProfileService_UserHasProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to برچسب مورد نظر یافت نشد..
        /// </summary>
        public static string TagService_TagNotFound {
            get {
                return ResourceManager.GetString("TagService_TagNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شما به این عنوان دسترسی ندارید..
        /// </summary>
        public static string TopicService_NoAccess {
            get {
                return ResourceManager.GetString("TopicService_NoAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این عنوان هیچ پستی ندارد و قابل انتشار نیست..
        /// </summary>
        public static string TopicService_NoPostToPublish {
            get {
                return ResourceManager.GetString("TopicService_NoPostToPublish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این عنوان هیچ پست عمومی ندارد و قابل انتشار نیست..
        /// </summary>
        public static string TopicService_NoPublicPostToPublish {
            get {
                return ResourceManager.GetString("TopicService_NoPublicPostToPublish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این عنوان هیچ مطلبی ندارد..
        /// </summary>
        public static string TopicService_TopicHasNoPost {
            get {
                return ResourceManager.GetString("TopicService_TopicHasNoPost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to عنوان مورد نظر یافت نشد..
        /// </summary>
        public static string TopicService_TopicNotFound {
            get {
                return ResourceManager.GetString("TopicService_TopicNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to پسورد جدید و قدیم یکی هستند..
        /// </summary>
        public static string UserService_ChangePasswordAsync_SamePassword {
            get {
                return ResourceManager.GetString("UserService_ChangePasswordAsync_SamePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این آدرس ایمیل قبلا ثبت شده است..
        /// </summary>
        public static string UserService_CreateAsync_EmailDuplicate {
            get {
                return ResourceManager.GetString("UserService_CreateAsync_EmailDuplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to طول پسورد حداقل {0} حرف باید باشد..
        /// </summary>
        public static string UserService_CreateAsync_PasswordLenght {
            get {
                return ResourceManager.GetString("UserService_CreateAsync_PasswordLenght", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این نام کاربری وجود دارد..
        /// </summary>
        public static string UserService_CreateAsync_UsernameDuplicate {
            get {
                return ResourceManager.GetString("UserService_CreateAsync_UsernameDuplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نام کاربری شما فعال نشده است. با ادمین سیستم تماس بگیرید..
        /// </summary>
        public static string UserService_LoginAsync_IsActive {
            get {
                return ResourceManager.GetString("UserService_LoginAsync_IsActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نام کاربری شما قفل است. برای رفع مشکل با ادمین سیستم تماس بگیرید..
        /// </summary>
        public static string UserService_LoginAsync_IsLock {
            get {
                return ResourceManager.GetString("UserService_LoginAsync_IsLock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شما بیش از حد مجاز پسورد اشتباه وارد کرده اید و نام کاربری شما موقتا قفل است. لطفا دیرتر تلاش کنید..
        /// </summary>
        public static string UserService_LoginAsync_LockDate {
            get {
                return ResourceManager.GetString("UserService_LoginAsync_LockDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نام کاربری یا رمز عبور وارد شده صحیح نیست..
        /// </summary>
        public static string UserService_LoginAsync_Wrong {
            get {
                return ResourceManager.GetString("UserService_LoginAsync_Wrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to کاربر مورد نظر یافت نشد..
        /// </summary>
        public static string UserService_UserNotFound {
            get {
                return ResourceManager.GetString("UserService_UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to کلمه عبور اشتباه است..
        /// </summary>
        public static string UserService_WrongPassword {
            get {
                return ResourceManager.GetString("UserService_WrongPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to رای مورد نظر یافت نشد..
        /// </summary>
        public static string VoteService_VoteNotFound {
            get {
                return ResourceManager.GetString("VoteService_VoteNotFound", resourceCulture);
            }
        }
    }
}
