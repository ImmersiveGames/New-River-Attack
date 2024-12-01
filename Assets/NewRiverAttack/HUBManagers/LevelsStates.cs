namespace NewRiverAttack.HUBManagers
{
    public enum LevelsStates
    {
        Locked,   // Não é possível acessar - Vermelho
        Actual,   // Level selecionado - Amarelo
        Open      // é possível retornar a estes nível já jogados - Branco
    }
}