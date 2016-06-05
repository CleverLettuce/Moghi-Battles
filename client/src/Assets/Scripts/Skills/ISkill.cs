using UnityEngine;
using System.Collections;
using System;

public interface ISkill {

    float getCooldown();
    float getLastFired();
    void fire();
    void interruptSkill();
    void onSkillCompleted(Action action);
    void onSkillInterrupted(Action action);
    string getName();
}
