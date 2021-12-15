﻿using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Logging;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace HeelsPlugin.Gui
{
  public class ConfigLine
  {
    private readonly int key = 0;
    private readonly ComboWithFilter<Item> combo;
    private readonly ConfigModel model;
    private string itemName
    {
      get {
        if (model == null || (model.ModelMain <=0 && model.Model <= 0)) return "";
        return combo.Items.Find(c =>
        {
          if (model.ModelMain > 0) return c.ModelMain == model.ModelMain;
          return (short)c.ModelMain == model.Model;
        }).Name.ToString();
      }
    }

    public int Key { get => key; }
    public ConfigModel Model { get => model; }

    public System.Action<int>? OnDelete;
    public System.Action? OnChange;

    public ConfigLine(int key, ComboWithFilter<Item> combo)
    {
      this.key = key;
      this.combo = combo;

      model = new()
      {
        Name = string.Empty,
        Model = 0,
        Offset = 0f
      };
    }

    public ConfigLine(int key, ComboWithFilter<Item> combo, ConfigModel model)
    {
      this.key = key;
      this.combo = combo;
      this.model = model;
    }

    public void Draw()
    {
      try
      {
        ImGui.BeginGroup();

        if (ImGui.Checkbox($"##Enabled{key}", ref model.Enabled))
        {
          OnChange?.Invoke();
        }

        ImGui.SameLine();
        ImGui.PushItemWidth(200f);
        if (ImGui.InputText($"##Name{key}", ref model.Name, 64))
        {
          OnChange?.Invoke();
        }
        ImGui.PopItemWidth();

        ImGui.SameLine();
        if (combo.Draw($"{itemName}##{key}", out var item))
        {
          model.ModelMain = item.ModelMain;
          OnChange?.Invoke();
        }

        ImGui.SameLine();
        ImGui.PushItemWidth(100);
        if (ImGui.InputFloat($"##Height{key}", ref model.Offset, 0.01f, 0.01f, "%.2f"))
        {
          OnChange?.Invoke();
        }
        ImGui.PopItemWidth();

        ImGui.SameLine();
        if (ImGuiComponents.IconButton(Key, FontAwesomeIcon.Trash))
        {
          OnDelete?.Invoke(key);
        }
      }
      finally
      {
        ImGui.EndGroup();
      }
    }
  }
}
