using UnityEngine;

public static class AnimatorExtensions
{
    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        foreach (var param in self.parameters)
        {
            if (param.name == name && param.type == type)
                return true;
        }
        return false;
    }
}
