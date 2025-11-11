using MyMusicBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicBook.Communication.Requests;

public class RequestRegisterUserJson
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;

}

