using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Description
    {
        public enum Condition
        {
            None,
            IsOpen,
            IsClosed,
            IsLocked,
            IsUnlocked,
            FirstTime,
            NotFirstTime
        }

        public static bool ParseCondition(string name, out Condition condition)
        {
            switch (name.ToLowerInvariant())
            {
                case "none": condition = Condition.None; return true;
                case "isopen": condition = Condition.IsOpen; return true;
                case "isclosed": condition = Condition.IsClosed; return true;
                case "islocked": condition = Condition.IsLocked; return true;
                case "isunlocked": condition = Condition.IsUnlocked; return true;
                case "firsttime": condition = Condition.FirstTime; return true;
                case "notfirsttime": condition = Condition.NotFirstTime; return true;
            }
            condition = Condition.None;
            return false;
        }

        public void Add(Condition condition, string text)
        {
            m_content.Add(new Para { Condition = condition, Text = text });
        }

        public void Describe(object owner)
        {
            var openable = owner as IOpenable;

            bool firstPara = true;

            foreach (var para in m_content)
            {
                if (TestCondition(openable, para.Condition))
                {
                    if (firstPara)
                    {
                        firstPara = false;
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    Helpers.WritePara(para.Text);
                }
            }

            m_firstTime = false;
        }

        struct Para
        {
            public Condition Condition;
            public string Text;
        }

        bool TestCondition(IOpenable openable, Condition condition)
        {
            switch (condition)
            {
                case Condition.None: return true;
                case Condition.IsOpen: return openable != null && openable.IsOpen;
                case Condition.IsClosed: return openable != null && !openable.IsOpen;
                case Condition.IsLocked: return openable != null && openable.IsLocked;
                case Condition.IsUnlocked: return openable != null && !openable.IsLocked;
                case Condition.FirstTime: return m_firstTime;
                case Condition.NotFirstTime: return !m_firstTime;
            }

            return true;
        }

        bool m_firstTime = true;
        List<Para> m_content = new List<Para>();
    }
}
