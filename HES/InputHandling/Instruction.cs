using HES.Interfaces;
using HES.Menus.Fields;
using System.Collections.Generic;
using System.Linq;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class Instruction : IResourceProvider
    {
        private List<VKObjectContainer> finalInstructions = new List<VKObjectContainer>();
        private List<string> cifs, startDates, endDates;

        private const string _RESOURCE = @"\Resources\instructions.json";

        public Instruction() { }

        public void SetInstructions(MenuFieldsContainer instructions)
        {
            finalInstructions.Add(VirtualKeys.SetVKs("./drv", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            SetLoginInstructionsImpl(instructions);

            SetAdditionalFieldsInstructionsImpl(instructions);

            for (int i = 0; i < cifs.Count; i++)
            {
                int numStartDates = startDates.Count.Equals(1) ? 0 : i; //If there's only 1 date use it for all the extracts
                int numEndDates = endDates.Count.Equals(1) ? 0 : i; 
                FormStmtSnap(cifs[i], startDates[numStartDates], endDates[numEndDates]);
            }

            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
        }

        private void SetLoginInstructionsImpl(MenuFieldsContainer instructions)
        {
            for (int i = 0; i < instructions.CountLoginFields(); i++)
            {
                MenuField field = instructions.LoginFields.ElementAt(i);
                finalInstructions.Add(VirtualKeys.SetVKs(field.GetValue<string>(), 0));
                finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            }
        }

        private void SetAdditionalFieldsInstructionsImpl(MenuFieldsContainer instructions)
        {
            //for (int i = 0; i < instructions.CountAdditionalFields(); i++)
            //{
                //MenuField field = instructions.AdditionalFields.ElementAt(i);
                cifs = instructions.AdditionalFields.ElementAt(0).GetValue<List<string>>();
                startDates = instructions.AdditionalFields.ElementAt(1).GetValue<List<string>>();
            endDates = instructions.AdditionalFields.ElementAt(2).GetValue<List<string>>();
            //}
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

        public void GetResource()
        {
            
        }
    }
}
