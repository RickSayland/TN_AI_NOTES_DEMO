using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TN_AI_NOTES_DEMO2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Treatment { get; set; }

        [BindProperty]
        public string Progress { get; set; }

        [BindProperty]
        public string Goals { get; set; }

        [BindProperty]
        public string Notes { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
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

            var result = GPT.Instance.Query(systemPrompt).Result;
            if (result == "\n\nOK")
            {
                UpdateForm(systemPrompt, result);
            }
            return RedirectToPage();

        }
        public IActionResult OnPostLessAngry()
        {
            var systemPrompt = "You will respond to all prompts with a less angry version of the prompt text - except for this first prompt. Respond with OK if you understand.";

            var result = GPT.Instance.Query(systemPrompt).Result;
            if (result == "\n\nOK")
            {
                UpdateForm(systemPrompt, result);
            }
            return RedirectToPage();
        }
        private void UpdateForm(string systemPrompt, string result)
        {
            Treatment = (Treatment != "") ? GPT.Instance.Query(systemPrompt + result + "\n\n" + Treatment).Result : "";
            Progress = (Progress != "") ? GPT.Instance.Query(systemPrompt + result + "\n\n" + Progress).Result : "";
            Goals = (Goals != "") ? GPT.Instance.Query(systemPrompt + result + "\n\n" + Goals).Result : "";
            Notes = (Notes != "") ? GPT.Instance.Query(systemPrompt + result + "\n\n" + Notes).Result : "";

            // For MCS save for the re-get
            TempData["Treatment"] = Treatment;
            TempData["Progress"] = Progress;
            TempData["Goals"] = Goals;
            TempData["Notes"] = Notes;
        }
    }
}