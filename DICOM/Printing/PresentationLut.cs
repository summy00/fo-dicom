﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dicom.Printing
{
    public class PresentationLut : DicomDataset
    {
        #region Public Properties

        public static readonly DicomUID SopClassUid = DicomUID.PresentationLUTSOPClass;

        public DicomUID SopInstanceUid { get; private set; }

        public DicomDataset LutSequence
        {
            get
            {
                var lutSequence = this.Get<DicomSequence>(DicomTag.PresentationLUTSequence);
                if (lutSequence != null)
                {
                    return lutSequence.Items[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public ushort[] LutDescriptor
        {
            get
            {
                if (LutSequence != null)
                {
                    return LutSequence.Get<ushort[]>(DicomTag.LUTDescriptor);
                }
                else
                {
                    return new ushort[0];
                }
            }
            set
            {
                if (LutSequence != null)
                {
                    LutSequence.Add(DicomTag.LUTDescriptor, value);
                }
                else
                {
                    throw new InvalidProgramException("No LUT sequence found, call CreateLutSequence first");
                }
            }
        }

        public string LutExplanation
        {
            get
            {
                return LutSequence != null 
                    ? LutSequence.Get(DicomTag.LUTDescriptor, string.Empty) 
                    : string.Empty;
            }
            set
            {
                if (LutSequence != null)
                    LutSequence.Add(DicomTag.LUTDescriptor, value);
                else
                    throw new InvalidProgramException("No LUT sequence found, call CreateLutSequence first");
            }
        }

        public ushort[] LutData
        {
            get
            {
                return LutSequence != null 
                    ? LutSequence.Get<ushort[]>(DicomTag.LUTData) 
                    : new ushort[0];
            }
            set
            {
                if (LutSequence != null)
                    LutSequence.Add(DicomTag.LUTData, value);
                else
                    throw new InvalidProgramException("No LUT sequence found, call CreateLutSequence first");
            }
        }

        public string PresentationLutShape
        {
            get { return Get(DicomTag.PresentationLUTShape, "IDENTITY"); }
            set { Add(DicomTag.PresentationLUTShape, value); }
        }

        #endregion

        #region Constrctuors

        public PresentationLut(DicomUID sopInstance = null)
        {
            InternalTransferSyntax = DicomTransferSyntax.ExplicitVRLittleEndian;
            if (sopInstance == null || sopInstance.UID == string.Empty)
                SopInstanceUid = DicomUID.Generate();
            else
                SopInstanceUid = sopInstance;
            CreateLutSequence();
        }

        public PresentationLut(DicomUID sopInstance, DicomDataset dataset)
        {
            if (dataset == null)
            {
                throw new ArgumentNullException("dataset");
            }
            SopInstanceUid = sopInstance;
            dataset.CopyTo(this);

        }

        public void CreateLutSequence()
        {
            var lutSequence = new DicomSequence(DicomTag.PresentationLUTSequence);
            lutSequence.Items.Add(new DicomDataset());
            this.Add(lutSequence);
        }

        #endregion
    }
}
