# SPDX-License-Identifier: AGPL-3.0-or-later

click-damage-modifier = Light (left click) attacks are { $arg ->
        [1] [color=green]{$abs}% stronger.[/color]
        *[other] [color=red]{$abs}% weaker.[/color]
    }

heavy-damage-modifier = Heavy (right click) attacks are { $arg ->
        [1] [color=green]{$abs}% stronger.[/color]
        *[other] [color=red]{$abs}% weaker.[/color]
    }
