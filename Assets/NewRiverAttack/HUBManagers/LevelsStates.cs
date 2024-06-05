namespace NewRiverAttack.HUBManagers
{
    public enum LevelsStates
    {
        Locked,   // Não é possível acessar - Vermelho
        Actual,   // Level selecionado - Amarelo
        Complete, // nível foi jogado e deve ser concluído assim que o jogador voltar pra HUB destruir pónte etc. - temp Verde
        Open      // é possível retornar a estes nível já jogados - Branco
    }
}