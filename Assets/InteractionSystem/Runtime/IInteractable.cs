namespace InteractionSystem
{
    public interface IInteractable
    {
        public void Interact();
        
        public void GainFocus();
        
        public void LoseFocus();

        public string GetInteractionPrompt();
    }
}