﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
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
        ///   Looks up a localized string similar to فایل مورد نظر یافت نشد..
        /// </summary>
        public static string FileService_FileNotFount {
            get {
                return ResourceManager.GetString("FileService_FileNotFount", resourceCulture);
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
        ///   Looks up a localized string similar to این کاربر پروفایل دارد..
        /// </summary>
        public static string ProfileService_UserHasProfile {
            get {
                return ResourceManager.GetString("ProfileService_UserHasProfile", resourceCulture);
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
    }
}
