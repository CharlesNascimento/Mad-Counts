# MadCounts — migração Built-in 2D para URP 2D

## Objetivo

Migrar o projeto para o Universal Render Pipeline compatível com Unity `6000.5.6f1`, preservando as cenas de produção, sprites, Canvas e lógica de gameplay.

## Escopo e critérios

- Usar o pacote URP embutido `17.5.0`.
- Criar/ativar `Renderer2DData` e `UniversalRenderPipelineAsset` em `Assets/_Project/Settings/URP`.
- Executar o conversor oficial Built-in 2D → URP 2D para materiais e referências.
- Não modificar scripts de gameplay nem cenas de demonstração de terceiros.
- Validar manifest/lock, assets URP, referência em `GraphicsSettings` e compilação sem erros.
- Play Mode e validação visual em `MainMenu`/`Game` continuam sendo uma etapa manual do Editor.

## Tarefas

- [x] Inventariar pipeline, cenas, materiais e APIs Built-in.
- [x] Confirmar URP `17.5.0` no Editor `6000.5.6f1`.
- [x] Adicionar pacote URP ao manifest.
- [x] Criar/ativar renderer 2D por migrador idempotente.
- [x] Executar conversor Built-in 2D → URP 2D.
- [x] Auditar referências e erros de compilação.
- [x] Registrar pendências de validação visual/Play Mode.

## Resultado da validação

- Unity batchmode concluiu com `MADCOUNTS_URP_VALIDATION: PASS`.
- Não houve erros `CS####` ou falha de compilação no log de validação.
- Os seis assets URP de qualidade gerados pelo conversor compartilham os mesmos defaults; o ajuste fino de performance por nível continua pendente.
- Permanecem avisos preexistentes de APIs obsoletas em K-Animator/I2 e o aviso do TMP sobre `Units Per EM`; eles não fazem parte desta migração.
- A validação visual e Play Mode das cenas `MainMenu` e `Game` ainda deve ser feita manualmente no Editor.

## Decisões

O projeto é essencialmente 2D: as cenas de produção usam câmera ortográfica, `SpriteRenderer`, Canvas e TextMeshPro, sem callbacks Built-in como `OnRenderImage`, `CommandBuffer` ou `Graphics.Blit`. Por isso o renderer 2D é a opção de menor impacto e mantém suporte futuro a `Light2D`.

O ponto de retorno versionado da conversão é o commit `b27ef5a`. A conversão oficial pode alterar referências de materiais de forma irreversível; o migrador não remove assets e é seguro executar novamente.
