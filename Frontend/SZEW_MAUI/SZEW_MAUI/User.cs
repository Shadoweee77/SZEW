using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SZEW_MAUI
{
    class User {
        public int id { get; private set; }
        public string name { get; private set; }
        public string surname { get; private set; }
        public int userType { get; private set; }
        public string typeName {
            get {
                if(userType == 0)
                    return "administrator";
                else if(userType == 1)
                    return "mechanik";
                else
                    return "inny";
            }
        }
        public User(int id, string name, string surname, int userType) {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.userType = userType;
        }
    }
}
