﻿using System;

public class DevelopHint
{
    public static void ToDo(string desc = "") { }

    public static void RangeStart(string desc = "") { }
    public static void RangeEnd(string desc = "") { }

    //public static void ComementOutStart(string desc = "") { }
    //public static void ComementOutEnd(string desc = "") { }
}

public class ToDoAttribute : Attribute{ public ToDoAttribute(string desc = "") { } }