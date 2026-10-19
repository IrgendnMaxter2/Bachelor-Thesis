using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private enum Outcome
    {
        GameWon
    }

    [SerializeField] private EndGameUI endGameUI;
    [SerializeField] private Outcome outcome = Outcome.GameWon;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        if (endGameUI == null)
        {
            Debug.LogError("Assign an EndGameUI component in the Inspector.", this);
            return;
        }

        triggered = true;

        if (outcome == Outcome.GameWon)
            endGameUI.ShowGameWon();
    }
}