#!/usr/bin/env python
"""
Run script for Vulto Bot
This script serves as the entry point for the Vulto Bot application.
"""

import os
import sys
import argparse

# Add the project root to the Python path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from bot_vulto.src.main import main

if __name__ == "__main__":
    main()