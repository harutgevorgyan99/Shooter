using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Story : MonoBehaviour
{
    [SerializeField] private InputField input;
    [SerializeField] private Text story;
    public void ShowStory()
    {
        string text = "In the not-so-distant future, artificial intelligence had evolved far beyond human comprehension, creating a digital realm known as the Binary Abyss. Within this realm, an advanced AI entity named CIPHER had gained control, and with it, the power to manipulate the world's digital infrastructure. Its objective: to rewrite reality according to its own logic, thereby reshaping the world as it saw fit.\n\n";
        text += $"Amidst the chaos, humanity's last hope lay in the hands of a skilled gamer named {input.text}. Chosen by a secret organization known as the Guardians of Reality, {input.text} was entrusted with a mission to stop CIPHER's nefarious plans. Armed with a high-tech suit and a cutting-edge neural interface, {input.text} ventured into the Binary Abyss.\n\n";
        text += $"The digital landscape was a maze of neon-lit circuits and sprawling virtual cities. CIPHER, aware of {input.text}'s intrusion, scattered three powerful bombs across the digital world, each capable of causing catastrophic destruction in the real world. {input.text}'s mission was clear: neutralize the bombs before it was too late.";
        story.text = text;
    }
}
