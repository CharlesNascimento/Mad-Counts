# Design da migração URP

```text
manifest (URP 17.5.0)
          |
          v
   Renderer2DData + URP Asset
          |
          v
GraphicsSettings.defaultRenderPipeline
          |
          v
BuiltInToURP2D material/reference converter
```

O script `Assets/Editor/MadCountsUrpMigration.cs` cria os assets somente quando ausentes, reutiliza os existentes, ativa o pipeline padrão e chama o conversor oficial. A operação não remove arquivos e não altera as cenas diretamente além das referências de material que o conversor oficial encontrar.
