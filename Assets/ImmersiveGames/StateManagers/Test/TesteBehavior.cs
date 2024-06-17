using System.Linq;
using UnityEngine;

namespace ImmersiveGames.StateManagers.Test
{
    public class TestBehavior : MonoBehaviour
    {
        void Start()
        {
            // Criando comportamentos de teste
            var subSubBehavior1 = new BehaviorTeste { Name = "SubSubBehavior1", Finalization = true, SubBehaviors = null };
            var subSubBehavior2 = new BehaviorTeste { Name = "SubSubBehavior2", Finalization = false, SubBehaviors = null };
            var subBehavior1 = new BehaviorTeste { Name = "SubBehavior1", Finalization = false, SubBehaviors = new IBehaviorTeste[] { subSubBehavior1, subSubBehavior2 } };
            var subBehavior2 = new BehaviorTeste { Name = "SubBehavior2", Finalization = true, SubBehaviors = null };
            var behavior = new BehaviorTeste { Name = "Behavior", Finalization = false, SubBehaviors = new IBehaviorTeste[] { subBehavior1, subBehavior2 } };

            // Testando a função de verificação de finalização dos subcomportamentos
            CheckSubBehaviorsFinalization(behavior);

            // Verificando o resultado na Unity Console
            PrintAllBehaviors(behavior);
        }

        private static void CheckSubBehaviorsFinalization(IBehaviorTeste behavior)
        {
            if (behavior.SubBehaviors is { Length: > 0 })
            {
                var allSubBehaviorsFinalized = behavior.SubBehaviors.All(subBehavior =>
                {
                    CheckSubBehaviorsFinalization(subBehavior);
                    return subBehavior.Finalization;
                });

                behavior.Finalization = allSubBehaviorsFinalized;
            }
            else
            {
                behavior.Finalization = true;
            }

            Debug.Log($"Comportamento '{behavior.Name}' finalizado: {behavior.Finalization}");
        }

        private void PrintAllBehaviors(IBehaviorTeste behavior)
        {
            Debug.Log("Todos os comportamentos e seus status de finalização:");
            PrintBehavior(behavior, 0);
        }

        private void PrintBehavior(IBehaviorTeste behavior, int level)
        {
            string indent = new string(' ', level * 2);
            Debug.Log($"{indent}{behavior.Name} - Finalizado: {behavior.Finalization}");

            if (behavior.SubBehaviors != null)
            {
                foreach (var subBehavior in behavior.SubBehaviors)
                {
                    PrintBehavior(subBehavior, level + 1);
                }
            }
        }
    }
    public interface IBehaviorTeste
    {
        string Name { get; set; }
        bool Finalization { get; set; }
        IBehaviorTeste[] SubBehaviors { get; set; }
    }
    public class BehaviorTeste : IBehaviorTeste
    {
        public string Name { get; set; }
        public bool Finalization { get; set; }
        public IBehaviorTeste[] SubBehaviors { get; set; }
    }
}