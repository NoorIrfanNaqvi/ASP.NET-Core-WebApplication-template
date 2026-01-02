using ASPlogInV2.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace ASPlogInV2.Helpers
{
    using BCrypt.Net;
    public class RegistorAccountHelper
    {
        //Checks if all the values are not null
        public static bool isValuesNull(string useremail, string username, string password, string confirmPassword) 
        {
            if (useremail == null || username == null || password == null || confirmPassword == null) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Adds account to database
        public static bool isNewAccountAdded(string email, string username, string password)
        {
            try 
            {
                //Gets data and set it into a database table format.
                using (var db = new DbConnectorSQLite())
                {
                    //Hash password
                    var hashedPassword = BCrypt.HashPassword(password);
                    //Creating a new account details into a table row to be added into the database
                    userAccounts NewAccount = new userAccounts()
                    {
                        UserEmail = email,
                        UserName = username,
                        Password = hashedPassword
                    };

                    db.UserAccounts.Add(NewAccount);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return false;
            }
        }

        //Check if username is taken
        public static bool isValueInDb(string value, string dbColumn)
        {
            //Trying to open the database
            try 
            {
                using (var db = new DbConnectorSQLite())
                {
                    //If function dbColumn wants to check username column
                    var userExist = db.UserAccounts.FirstOrDefault();
                    if (dbColumn == "username") 
                    {
                        userExist = db.UserAccounts.FirstOrDefault(n => n.UserName == value);
                    }
                    else if (dbColumn == "useremail")
                    {
                        userExist = db.UserAccounts.FirstOrDefault(e => e.UserEmail == value);
                    }
                    //You want to have more no duplicate column values? Uncomment the code below and replace the values in []
                    /*
                    else if (dbColumn == "[TableColumn you want to check]")
                    {
                    userExist = db.UserAccounts.FirstOrDefault(u => u.[TableColumnToBeChecked] == value);
                    }
                     */

                    //Not null means value is in database
                    if (userExist != null)
                    {
                        //Username is taken
                        return true;
                    }
                    //Null means value not in database
                    else
                    {
                        //Username can be used
                        return false;
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return true;
            }
        }

        //Minimum password requirements
        public static bool isPasswordGood(string userPassword)
        {
            try 
            {
                //Checks if string has at least one digit
                bool hasDigit = false;
                bool haslowerchar = false;
                bool hasupperchar = false;
                foreach (char c in userPassword) 
                {
                    //Check if there is a number
                    if (char.IsDigit(c))
                    {
                        hasDigit = true;
                    }
                    //Check if there is a lowercase letter
                    if (char.IsLower(c))
                    {
                        haslowerchar = true;
                    }
                    //Check if there is an uppercase letter
                    if (char.IsUpper(c))
                    {
                        hasupperchar = true;
                    }

                    //To stop once digit, lower and upper case requirements are found.
                    if (hasDigit && haslowerchar && hasupperchar)
                    {
                        break;
                    }
                }

                //If password valid
                if (userPassword.Count() >= 8 && hasDigit && haslowerchar && hasupperchar) 
                {
                    return true;
                }
                //Pasword doesn't meet minimum requirements
                else 
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return false;
            }
        }

        //Varifies the if data in the inputs are valid
        public static string validatingAccountCreation(CheckRegistrationData NewUserDetails)
        {
            //Check if values are not null
            try 
            {
                bool nullInputs = isValuesNull(NewUserDetails.useremail, NewUserDetails.username, NewUserDetails.password, NewUserDetails.confirmpassword);
                if (!nullInputs) 
                {
                    //Check if username is taken
                    bool usernameTaken = isValueInDb(NewUserDetails.username, "username");
                    //Check if email is taken
                    bool emailTaken = isValueInDb(NewUserDetails.useremail, "useremail");
                    //Check if password matches confirmed password
                    bool passwordsMatch = NewUserDetails.password == NewUserDetails.confirmpassword;
                    //Is password follow minmum rule
                    bool isGoodPassword = isPasswordGood(NewUserDetails.password);

                    //Checks if all the details are in a valid formate and the key values aren't already used
                    //If all values valid
                    if (!usernameTaken && !emailTaken && passwordsMatch && isGoodPassword) 
                    {
                        bool createAccount = isNewAccountAdded(NewUserDetails.useremail, NewUserDetails.username, NewUserDetails.password);
                        if (createAccount)
                        {
                            //Account created successfully
                            return "";
                        }
                        //Account creation failed vvv
                        else
                        {
                            return "Something went wrong, couldn't create account, please try again.";
                        }
                    }
                    else if (usernameTaken)
                    {
                        return "Sorry but this username is already taken.";
                    }
                    else if (emailTaken)
                    {
                        return "Sorry this email has already been used to create an account, if username was forgotten use your email in the place of the username.";
                    }
                    else if (!passwordsMatch)
                    {
                        return "Password does not match confirm password.";
                    }
                    else if (!isGoodPassword)
                    {
                        return "The password must be at least 8 characters long, have one digit and one uppercase and lowercase letters.";
                    }
                    else
                    {
                        return "Something went wrong.";
                    }
                }
                else 
                {
                    return "Else";
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return "Something went wrong.";
            }
        }
    }
}
