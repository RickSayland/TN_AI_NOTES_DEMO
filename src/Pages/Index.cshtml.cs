using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TN_AI_NOTES_DEMO;

namespace TN_AI_NOTES_DEMO2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IArtificialIntelligence _AI;

        [BindProperty]
        public string Treatment { get; set; }

        [BindProperty]
        public string Progress { get; set; }

        [BindProperty]
        public string Goals { get; set; }

        [BindProperty]
        public string Notes { get; set; }

        public IndexModel()
        {
            _AI = GPT.Instance;

            Treatment = "This patient needs help. They are annoying and keep telling me they see ghosts. They should take ibuprofen 200mg daily to fix this.";
            Progress = "The patient is making terrible progress. They have a new best friend named Casper.";
            Goals = "The patient should aim to see less ghosts.";
            Notes = "Try not to frighten the pateint. Dont wear white lab coats.";
        }

        public void OnGet()
        {
            if (TempData.ContainsKey("Treatment")) Treatment = TempData["Treatment"].ToString();
            if (TempData.ContainsKey("Progress")) Progress = TempData["Progress"].ToString();
            if (TempData.ContainsKey("Goals")) Goals = TempData["Goals"].ToString();
            if (TempData.ContainsKey("Notes")) Notes = TempData["Notes"].ToString();
        }

        public void OnPost()
        {

        }

        public IActionResult OnPostMoreAngry()
        {
            var systemPrompt = "You will respond to all prompts with an angrier version of the prompt text - except for this first prompt. Respond with OK if you understand.";

            var result = _AI.Query(systemPrompt).Result;
            if (result == "\n\nOK")
            {
                UpdateForm(systemPrompt, result);
            }
            return RedirectToPage();

        }
        public IActionResult OnPostLessAngry()
        {
            var systemPrompt = "You will respond to all prompts with a less angry version of the prompt text - except for this first prompt. Respond with OK if you understand.";

            var result = _AI.Query(systemPrompt).Result;
            if (result == "\n\nOK")
            {
                UpdateForm(systemPrompt, result);
            }
            return RedirectToPage();
        }
        private void UpdateForm(string systemPrompt, string result)
        {
            Treatment = (Treatment != "") ? _AI.Query(systemPrompt + result + "\n\n" + Treatment).Result : "";
            Progress = (Progress != "") ? _AI.Query(systemPrompt + result + "\n\n" + Progress).Result : "";
            Goals = (Goals != "") ? _AI.Query(systemPrompt + result + "\n\n" + Goals).Result : "";
            Notes = (Notes != "") ? _AI.Query(systemPrompt + result + "\n\n" + Notes).Result : "";

            // For MVC save for the re-get
            TempData["Treatment"] = Treatment.Replace("\n", "");
            TempData["Progress"] = Progress.Replace("\n", "");
            TempData["Goals"] = Goals.Replace("\n", "");
            TempData["Notes"] = Notes.Replace("\n", "");
        }
    }
}