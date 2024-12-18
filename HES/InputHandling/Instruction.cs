using System.Collections.Generic;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class Instruction
    {
        private List<VKObjectContainer> finalInstructions = new List<VKObjectContainer>();
        private string[] cifs;
        private string[] startDates;
        private string[] endDates;

        public Instruction(HESFile fileVKs) { }

        public void SetInstructions(Dictionary<string, string> instructions)
        {
            finalInstructions.Add(VirtualKeys.SetVKs("./drv", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(instructions["Username"], 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(instructions["Password"], 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            cifs = instructions["Cifs"].TrimEnd(' ').Split(' ');
            startDates = instructions["Start Date"].TrimEnd(' ').Split(' ');
            endDates = instructions["End Date"].TrimEnd(' ').Split(' ');

            for (int i = 0; i < cifs.Length; i++)
            {
                int numStartDates = startDates.Length.Equals(1) ? 0 : i; //If there's only 1 date use it for all the extracts
                int numEndDates = endDates.Length.Equals(1) ? 0 : i; 
                FormStmtSnap(cifs[i], startDates[numStartDates], endDates[numEndDates]);
            }

            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
        }

        private void FormStmtSnap(string cif, string startDate, string endDate)
        {
            finalInstructions.Add(VirtualKeys.SetVKs("FORMSTMTSNAP", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.END, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(cif, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs("1", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(startDate, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(endDate, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs("2", 0));

            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
        }

        public List<VKObjectContainer> GetInstructions()
        {
            return finalInstructions;
        }

        public void Clear()
        {
            finalInstructions.Clear();
        }
    }
}
