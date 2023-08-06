using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TN_AI_NOTES_DEMO;

namespace TN_AI_NOTES_DEMO.Pages
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
            _AI.SetSystemMessage("You will respond to all prompts with an angrier version of the prompt text");

            UpdateForm();

            return RedirectToPage();

        }
        public IActionResult OnPostLessAngry()
        {
            _AI.SetSystemMessage("You will respond to all prompts with a less angry version of the prompt text");

            UpdateForm();

            return RedirectToPage();
        }
        private void UpdateForm()
        {
            Treatment = (Treatment != "") ? _AI.Query(Treatment).Result : "";
            Progress = (Progress != "") ? _AI.Query(Progress).Result : "";
            Goals = (Goals != "") ? _AI.Query(Goals).Result : "";
            Notes = (Notes != "") ? _AI.Query(Notes).Result : "";

            // For MVC save for the re-get
            TempData["Treatment"] = Treatment;
            TempData["Progress"] = Progress;
            TempData["Goals"] = Goals;
            TempData["Notes"] = Notes;
        }
    }
}