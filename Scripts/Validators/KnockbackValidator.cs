//-----------------------------------------------------------------------
// <copyright file="KnockbackValidator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Validators
{
    public class KnockbackValidator : IValidator
    {
        bool IValidator.IsValid()
        { 
            return true;
        }
    }
}